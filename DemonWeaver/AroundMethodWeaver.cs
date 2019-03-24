using System.Linq;
using DemonWeaver.Extensions;
using Mono.Cecil;
using Mono.Cecil.Cil;

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

        public AroundMethodWeaver(DemonTypes demonTypes, MethodDefinition target, MethodReference advice, FieldDefinition adviceField)
        {
            _demonTypes = demonTypes;
            _target = target;
            _advice = advice;
            _adviceField = adviceField;
            _il = target.Body.GetILProcessor();
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

            var joinPoint = _target.Module.ImportReference(_demonTypes.JoinPoint.MakeGenericType(parameterGeneric, returnGeneric));
            var joinPointConstructor = _target.Module.ImportReference(_demonTypes.JoinPointConstructor.MakeGeneric(parameterGeneric, returnGeneric));

            ClearBody();

            var joinPointVariable = new VariableDefinition(joinPoint);

            _target.Body.Variables.Add(joinPointVariable);

            InsertLoadParameters(parameterGeneric);
            InsertLoadReturn(returnGeneric);
            Append(_il.Create(OpCodes.Ldnull));
            Append(_il.Create(OpCodes.Newobj, joinPointConstructor));
            Append(_il.Create(OpCodes.Stloc_0));
            Append(_il.Create(OpCodes.Ldarg_0));
            Append(_il.Create(OpCodes.Ldfld, _adviceField));
            Append(_il.Create(OpCodes.Ldloc_0));
            Append(_il.Create(OpCodes.Call, _advice)); //todo callvirt if needed 
            InsertRetWithReturnValue(parameterGeneric, returnGeneric);
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
            Append(_il.Create(OpCodes.Ldstr, firstParameter.Name));

            var parametersTypeGeneric = ((GenericInstanceType) parametersType).GenericArguments[0];

            var constructor = _demonTypes.Parameters.GenericConstructor1
                .MakeGeneric(parametersTypeGeneric)
                .Let(_target.Module.ImportReference);

            Append(_il.Create(OpCodes.Newobj, constructor));
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

            Append(_il.Create(OpCodes.Newobj, _target.Module.ImportReference(constructor)));
        }

        //todo test this
        void InsertRetWithReturnValue(TypeReference parameterGeneric, TypeReference returnGeneric)
        {
            if (returnGeneric.GetElementType().FullName == DemonTypes.FullNames.ReturnFullNames.Void)
            {
                Append(_il.Create(OpCodes.Ldnull)); //todo is this needed?
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

                Append(_il.Create(OpCodes.Ldloc_0));
                Append(_il.Create(OpCodes.Call, accessReturn));
                Append(_il.Create(OpCodes.Call, accessValue));
            }

            Append(_il.Create(OpCodes.Ret));
        }

        void Append(Instruction instruction) => _il.Append(instruction);
    }
}