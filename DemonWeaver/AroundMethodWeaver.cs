using System;
using System.Linq;
using System.Runtime.CompilerServices;
using DemonWeaver.Extensions;
using DemonWeaver.IlEmitter;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace DemonWeaver
{
    //todo impl and don't copy paste
    //todo either make another class with common or merge this and BeforeMethodWeaver
    public class AroundMethodWeaver
    {
        readonly DemonTypes _demonTypes;
        readonly MethodDefinition _target;
        readonly MethodReference _advice;
        readonly FieldDefinition _adviceField;
        readonly ILProcessor _il;
        readonly Emitter _emitter;

        public AroundMethodWeaver(DemonTypes demonTypes, MethodDefinition target, MethodReference advice, FieldDefinition adviceField)
        {
            _demonTypes = demonTypes;
            _target = target;
            _advice = advice;
            _adviceField = adviceField;
            _il = target.Body.GetILProcessor();
            _emitter = EmitterFactory.GetAppend(_il);
        }

        public void Weave()
        {
            var joinPoint = GetJoinPoint();

            var joinPointType = (GenericInstanceType) joinPoint.ParameterType;

            if (joinPointType.GetElementType().FullName == DemonTypes.FullNames.JoinPoint)
                WeaveSync(joinPointType);
        }

        //todo allow a typejoinpoint, either inside the joinPoint or as a separate parameter in the aspect
        ParameterDefinition GetJoinPoint()
        {
            if (_advice.Parameters.Count == 1)
                return _advice.Parameters[0];
            throw new WeavingException("Around advice must have only one parameter, which is a JoinPoint"); //todo add context if missing, make nicer
        }

        //todo target should be sync, filter 
        //todo test params[]
        //todo is in, out, ref hard to impl
        void WeaveSync(GenericInstanceType joinPointType)
        {
            var parameterGeneric = joinPointType.GenericArguments[0];
            var returnGeneric = joinPointType.GenericArguments[1];

            var joinPoint = _target.Module.ImportReference(_demonTypes.JoinPoint.MakeGenericInstanceType(parameterGeneric, returnGeneric));
            var joinPointConstructor = _target.Module.ImportReference(_demonTypes.JoinPointConstructor.MakeGeneric(parameterGeneric, returnGeneric));

            var (cachedDelegateTypeField, cachedDelegateField) = CreateLambdaType(parameterGeneric, returnGeneric);

            ClearBody();

            var joinPointVariable = new VariableDefinition(joinPoint);

            _target.Body.Variables.Add(joinPointVariable);

            InsertLoadParameters(parameterGeneric);
            InsertLoadReturn(returnGeneric);
            InsertLambda(parameterGeneric, returnGeneric, cachedDelegateTypeField, cachedDelegateField);
            _emitter.Newobj(joinPointConstructor);
            _emitter.Stloc_0();
            _emitter.Ldarg_0();
            _emitter.Ldfld(_adviceField);
            _emitter.Ldloc_0();
            _emitter.Call(_advice); //todo callvirt if needed 
            InsertRetWithReturnValue(parameterGeneric, returnGeneric);
        }

        //todo idea: naming tool like https://github.com/dotnet/roslyn/blob/master/src/Compilers/CSharp/Portable/Symbols/Synthesized/GeneratedNames.cs?
        //todo hardcoded name, might be conflicts
        //todo rev eng lambdas in situations such as: multiple lambdas of the same type, closures
        //todo can lambds be shared?
        //todo rev eng do TypeAttributes ever differ?
        //todo split in more methods or class 
        //todo clean this up
        //todo idea: store common primitives definitions such as type of void, object constructor
        //todo rename cachedDelegateTypeField and cachedDelegateField since they sound alike
        (FieldDefinition cachedDelegateTypeField, FieldDefinition cachedDelegateField) CreateLambdaType(TypeReference parametersType, TypeReference returnType)
        {
            var type = new TypeDefinition(
                "",
                "<Demon<>c",
                TypeAttributes.Public | TypeAttributes.NestedPublic | TypeAttributes.Sealed | TypeAttributes.Serializable | TypeAttributes.BeforeFieldInit,
                _target.Module.TypeSystem.Object);

            //todo move somewhere if used more
            var compilerGeneratedAttributeConstructor = _target.Module.ImportReference(
                typeof(CompilerGeneratedAttribute)
                    .GetConstructors().First());

            type.CustomAttributes.Add(new CustomAttribute(compilerGeneratedAttributeConstructor));

            //todo do i need to import self?
            var cachedDelegateTypeField = new FieldDefinition(
                "<Demon<>9",
                FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.Static | FieldAttributes.InitOnly,
                type);

            type.Fields.Add(cachedDelegateTypeField);

            var ctor = new MethodDefinition(
                ".ctor",
                MethodAttributes.FamORAssem | MethodAttributes.Family | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                _target.Module.TypeSystem.Void);

            var ctorEmitter = ctor.Body.GetILProcessor().Let(EmitterFactory.GetAppend);

            ctorEmitter.Ldarg_0();
            ctorEmitter.Call(_target.Module.ImportReference(typeof(object).GetConstructors().First()));
            ctorEmitter.Ret();

            type.Methods.Add(ctor);

            var cctor = new MethodDefinition(
                ".cctor",
                MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                _target.Module.TypeSystem.Void);

            var cctorEmitter = cctor.Body.GetILProcessor().Let(EmitterFactory.GetAppend);

            cctorEmitter.Newobj(ctor);
            cctorEmitter.Stsfld(cachedDelegateTypeField);
            cctorEmitter.Ret();

            var cachedDelegateFieldType = _target.Module.ImportReference(typeof(Action<,>))
                .MakeGenericInstanceType(parametersType, returnType);

            var cachedDelegateField = new FieldDefinition(
                "<Demon<>9__3_0",
                FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.Static,
                cachedDelegateFieldType);

            type.Fields.Add(cachedDelegateField);

            var delegateMethod = new MethodDefinition(
                "<Demon<" + _target.Name + ">b__3_0",
                MethodAttributes.Private | MethodAttributes.FamANDAssem | MethodAttributes.HideBySig,
                _target.Module.TypeSystem.Void);

            var delegateMethodParametersParameter = new ParameterDefinition(
                "parameters",
                ParameterAttributes.None,
                parametersType);

            var delegateMethodReturnParameter = new ParameterDefinition(
                "ret",
                ParameterAttributes.None,
                returnType);

            delegateMethod.Parameters.Add(delegateMethodParametersParameter);
            delegateMethod.Parameters.Add(delegateMethodReturnParameter);

            var delegateMethodEmitter = delegateMethod.Body.GetILProcessor().Let(EmitterFactory.GetAppend);

//todo add code from original method here
            delegateMethodEmitter.Ret();

            type.Methods.Add(delegateMethod);

            _target.DeclaringType.NestedTypes.Add(type);

            return (cachedDelegateTypeField, cachedDelegateField);
        }

//todo does anything else need clearing?
        void ClearBody()
        {
            _target.Body.Variables.Clear();
            _target.Body.Instructions.Clear();
        }

        //todo unit test all cases
        //todo impl others beside generic1
        void InsertLoadParameters(TypeReference parametersType)
        {
            var firstParameter = _target.Parameters.First();

            Append(_il.GetEfficientLoadInstruction(firstParameter));
            _emitter.Ldstr(firstParameter.Name);

            var parametersTypeGeneric = ((GenericInstanceType) parametersType).GenericArguments[0];

            var constructor = _demonTypes.Parameters.GenericConstructor1
                .MakeGeneric(parametersTypeGeneric)
                .Let(_target.Module.ImportReference);

            _emitter.Newobj(constructor);
        }

        //todo unit test all cases
        void InsertLoadReturn(TypeReference reference)
        {
            MethodReference constructor = default;

            switch (reference.GetElementType().FullName)
            {
                case DemonTypes.FullNames.ReturnFullNames.Any:
                    constructor = _demonTypes.Returns.AnyConstructor;
                    break;
                case DemonTypes.FullNames.ReturnFullNames.Generic:
                {
                    var genericReturnType = ((GenericInstanceType) reference).GenericArguments[0];
                    constructor = _demonTypes.Returns.GenericConstructor.MakeGeneric(genericReturnType);
                    break;
                }
                case DemonTypes.FullNames.ReturnFullNames.Void:
                    constructor = _demonTypes.Returns.VoidConstructor;
                    break;
            }

            _emitter.Newobj(_target.Module.ImportReference(constructor));
        }

        void InsertLambda(TypeReference parametersType, TypeReference returnType, FieldDefinition cachedDelegateTypeField, FieldDefinition cachedDelegateField)
        {
            _emitter.Ldnull();
        }

        //todo test this
        void InsertRetWithReturnValue(TypeReference parameterGeneric, TypeReference returnGeneric)
        {
            if (returnGeneric.GetElementType().FullName == DemonTypes.FullNames.ReturnFullNames.Void)
            {
                _emitter.Ldnull(); //todo is this needed?
            }
            else
            {
                //todo get rid of getting the methodrefs here
                var accessReturn = _demonTypes
                    .JoinPoint
                    .Properties
                    .First(m => m.Name == "Return")
                    .GetMethod
                    .MakeGeneric(parameterGeneric, returnGeneric)
                    .Let(_target.Module.ImportReference);

                var returnGenericGeneric = ((GenericInstanceType) returnGeneric).GenericArguments[0];

                var accessValue = returnGeneric
                    .Resolve()
                    .Properties
                    .First(p => p.Name == "Value")
                    .GetMethod
                    .MakeGeneric(returnGenericGeneric)
                    .Let(_target.Module.ImportReference);

                _emitter.Ldloc_0();
                _emitter.Call(accessReturn);
                _emitter.Call(accessValue);
            }

            _emitter.Ret();
        }

        void Append(Instruction instruction) => _il.Append(instruction);
    }
}