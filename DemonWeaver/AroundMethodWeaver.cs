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
        readonly Instruction _originalFirstInstruction;

        public AroundMethodWeaver(DemonTypes demonTypes, MethodDefinition target, MethodReference advice, FieldDefinition adviceField)
        {
            _demonTypes = demonTypes;
            _target = target;
            _advice = advice;
            _adviceField = adviceField;
            _il = target.Body.GetILProcessor();
            _originalFirstInstruction = target.Body.Instructions[0];
        }

        public void Weave()
        {
            var joinPoint = GetJoinPoint();

            var joinPointType = (GenericInstanceType) joinPoint.ParameterType;

            if (joinPointType.GetElementType().FullName == DemonTypes.FullNames.JoinPoint)
                WeaveSync(joinPointType);
        }

        //todo target should be sync, filter 
        void WeaveSync(GenericInstanceType joinPointType)
        {
            var ctor = _demonTypes.JoinPointConstructor.MakeGeneric(joinPointType.GenericArguments[0], joinPointType.GenericArguments[1]);

            _target.Body.Instructions.Clear();

            _il.Append(_il.Create(OpCodes.Ldnull));
            _il.Append(_il.Create(OpCodes.Ldnull));
            _il.Append(_il.Create(OpCodes.Ldnull));
            _il.Append(_il.Create(OpCodes.Newobj, _target.Module.ImportReference(ctor)));
            _il.Append(_il.Create(OpCodes.Ldarg_0));
            _il.Append(_il.Create(OpCodes.Ldfld, _adviceField));
            _il.Append(_il.Create(OpCodes.Call, _advice));        //todo callvirt if needed 
            _il.Append(_il.Create(OpCodes.Ldnull));
            _il.Append(_il.Create(OpCodes.Ret));
        }

        //todo allow a typejoinpoint, either inside the joinPoint or as a separate parameter in the aspect
        ParameterDefinition GetJoinPoint()
        {
            if (_advice.Parameters.Count == 1)
                return _advice.Parameters[0];
            throw new WeavingException("Around advice must have only one parameter, which is a JoinPoint"); //todo add context if missing, make nicer
        }
    }
}