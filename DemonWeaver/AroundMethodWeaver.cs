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

        //todo verify if weaving doesn't break short addresses 
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

            var lambda = CreateLambda(parameterGeneric, returnGeneric);

            ClearBody();

            var joinPointVariable = new VariableDefinition(joinPoint);

            _target.Body.Variables.Add(joinPointVariable);

            InsertLoadParameters(parameterGeneric);
            InsertLoadReturn(returnGeneric);
            InsertLambdaAndCreateJoinPoint(parameterGeneric, returnGeneric, lambda);
            _emitter.Stloc_0();
            _emitter.Ldarg_0();
            _emitter.Ldfld(_adviceField);
            _emitter.Ldloc_0();
            _emitter.Call(_advice); //todo callvirt if needed 
            InsertRetWithReturnValue(parameterGeneric, returnGeneric);
        }

        //todo idea: naming tool like https://github.com/dotnet/roslyn/blob/master/src/Compilers/CSharp/Portable/Symbols/Synthesized/GeneratedNames.cs?
        //todo hardcoded name, might be conflicts
        //todo clean this up
        //todo idea: store common primitives definitions such as type of void, object constructor
        MethodDefinition CreateLambda(TypeReference parametersType, TypeReference returnType)
        {
            var lambdaMethod = new MethodDefinition(
                $"<Demon<{_target.Name}>b__6_0",
                MethodAttributes.Private | MethodAttributes.HideBySig,
                _target.Module.TypeSystem.Void);

            var parametersParameter = new ParameterDefinition(
                "parameters",
                ParameterAttributes.None,
                parametersType);

            var returnParameter = new ParameterDefinition(
                "ret",
                ParameterAttributes.None,
                returnType);

            lambdaMethod.Parameters.Add(parametersParameter);
            lambdaMethod.Parameters.Add(returnParameter);

            //todo move somewhere if used more
            var compilerGeneratedAttributeConstructor = _target.Module.ImportReference(
                typeof(CompilerGeneratedAttribute)
                    .GetConstructors()
                    .First());

            lambdaMethod.CustomAttributes.Add(new CustomAttribute(compilerGeneratedAttributeConstructor));

            var emitter = lambdaMethod.Body.GetILProcessor().Let(EmitterFactory.GetAppend);

            //todo crete a clone of the original method and call it here
            emitter.Ret();

            _target.DeclaringType.Methods.Add(lambdaMethod);

            return lambdaMethod;
        }

        //todo impl more parameter types rather than just 1
        void WeaveCloneAndCall(MethodDefinition lambda, TypeReference parametersType, TypeReference returnType)
        {
        }

        //todo naming
        //todo would target ever be static here?
        //todo check static
        //todo make private
        //todo check debug information
        MethodDefinition CloneTarget()
        {
            var cloned = new MethodDefinition(
                $"<Demon<{_target.Name}>Original",
                _target.Attributes,
                _target.MethodReturnType.ReturnType)
            {
                HasThis = _target.HasThis,
                ExplicitThis = _target.ExplicitThis,
                CallingConvention = _target.CallingConvention
            };

            foreach (var parameter in _target.Parameters)
                cloned.Parameters.Add(parameter);

            foreach (var variable in _target.Body.Variables)
                cloned.Body.Variables.Add(variable);

            foreach (var variable in _target.Body.ExceptionHandlers)
                cloned.Body.ExceptionHandlers.Add(variable);

            var targetProcessor = cloned.Body.GetILProcessor();
            foreach (var instruction in _target.Body.Instructions)
                targetProcessor.Append(instruction);

            if (_target.HasGenericParameters)
            {
                foreach (var parameter in _target.GenericParameters)
                {
                    var clonedParameter = new GenericParameter(parameter.Name, cloned);
                    if (parameter.HasConstraints)
                    {
                        foreach (var parameterConstraint in parameter.Constraints)
                        {
                            clonedParameter.Attributes = parameter.Attributes;
                            clonedParameter.Constraints.Add(parameterConstraint);
                        }
                    }

                    if (parameter.HasReferenceTypeConstraint)
                    {
                        clonedParameter.Attributes |= GenericParameterAttributes.ReferenceTypeConstraint;
                        clonedParameter.HasReferenceTypeConstraint = true;
                    }

                    if (parameter.HasNotNullableValueTypeConstraint)
                    {
                        clonedParameter.Attributes |= GenericParameterAttributes.NotNullableValueTypeConstraint;
                        clonedParameter.HasNotNullableValueTypeConstraint = true;
                    }

                    if (parameter.HasDefaultConstructorConstraint)
                    {
                        clonedParameter.Attributes |= GenericParameterAttributes.DefaultConstructorConstraint;
                        clonedParameter.HasDefaultConstructorConstraint = true;
                    }

                    cloned.GenericParameters.Add(clonedParameter);
                }
            }

            if (_target.DebugInformation.HasSequencePoints)
            {
                foreach (var sequencePoint in _target.DebugInformation.SequencePoints)
                    cloned.DebugInformation.SequencePoints.Add(sequencePoint);
            }

            cloned.DebugInformation.Scope = new ScopeDebugInformation(_target.Body.Instructions.First(), _target.Body.Instructions.Last());

            return cloned;
        }

        void ClearBody()
        {
            _target.Body.Variables.Clear();
            _target.Body.Instructions.Clear();
            _target.Body.ExceptionHandlers.Clear();
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

        void InsertLambdaAndCreateJoinPoint(TypeReference parametersType, TypeReference returnType, MethodDefinition lambdaMethod)
        {
            _emitter.Ldarg_0();
            _emitter.Ldftn(lambdaMethod);

            //todo get rid of getting the methodrefs here
            //todo MakeGenericInstanceType might not be needed
            var actionConstructor = _target.Module.ImportReference(typeof(Action<,>))
                .MakeGenericInstanceType(parametersType, returnType)
                .Resolve()
                .GetConstructors()
                .First()
                .MakeGeneric(parametersType, returnType)
                .Let(_target.Module.ImportReference);

            _emitter.Newobj(actionConstructor);

            var joinPointConstructor = _target.Module.ImportReference(_demonTypes.JoinPointConstructor.MakeGeneric(parametersType, returnType));

            _emitter.Newobj(joinPointConstructor);
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