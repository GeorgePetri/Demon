using System;
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
            var proceed = GetProceed();
            
            if(IsSync(proceed))
                WeaveSync(proceed);
            throw new NotImplementedException();
        }

        void WeaveSync(ParameterDefinition parameterDefinition)
        {
            
        }

        bool IsSync(ParameterDefinition parameterDefinition)
        {
            switch (parameterDefinition.ParameterType.FullName)
            {
                case DemonTypes.FullNames.SyncProceed:
                    return true;
                case DemonTypes.FullNames.AsyncProceed:
                    return false;
                default:
                    //todo copy pasted
                    throw new WeavingException("Around advice must have only one parameter, which is a JoinPoint"); //todo add context if missing, make nicer
            }
        }

        ParameterDefinition GetProceed()
        {
            if (_advice.Parameters.Count == 1)
                return _advice.Parameters[0];
            throw new WeavingException("Around advice must have only one parameter, which is a JoinPoint"); //todo add context if missing, make nicer
        }
    }
}