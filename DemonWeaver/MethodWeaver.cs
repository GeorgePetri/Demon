using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace DemonWeaver
{
    public class MethodWeaver
    {
        readonly MethodDefinition _target;
        readonly MethodDefinition _advice;
        readonly FieldDefinition _adviceField;
        readonly ILProcessor _il;
        readonly Instruction _originalFirstInstruction;

        public MethodWeaver(MethodDefinition target, MethodDefinition advice, FieldDefinition adviceField)
        {
            _target = target;
            _advice = advice;
            _adviceField = adviceField;
            _il = target.Body.GetILProcessor();
            _originalFirstInstruction = target.Body.Instructions[0];
        }

        public void Weave()
        {
            InsertLoadAspectIfNeeded();
            InsertLoadAdviceBoundParametersIfNeeded();

            var callAspect = _il.Create(OpCodes.Call, _advice);
            InsertBeforeOriginalFirst(callAspect);
        }

        void InsertLoadAspectIfNeeded()
        {
            if (_adviceField == null)
                return;

            var loadClass = _il.Create(OpCodes.Ldarg_0);
            var loadAspect = _il.Create(OpCodes.Ldfld, _adviceField);

            InsertBeforeOriginalFirst(loadClass);
            InsertBeforeOriginalFirst(loadAspect);
        }

        void InsertLoadAdviceBoundParametersIfNeeded()
        {
            if (!_advice.HasParameters)
                return;

            foreach (var parameter in _advice.Parameters)
            {
                var parameterToLoad = _target.Parameters.FirstOrDefault(p => p.ParameterType == parameter.ParameterType);
                if (parameterToLoad != null)
                {
                    var loadParameter = _il.GetEfficientLoadInstruction(parameter);
                    InsertBeforeOriginalFirst(loadParameter);
                }
                else
                {
                    //todo add nicer error message when GetLoadForDefault throws
                    var loadDefault = _il.GetLoadForDefault(parameter.ParameterType);
                    InsertBeforeOriginalFirst(loadDefault);
                }
            }
        }

        void InsertBeforeOriginalFirst(Instruction instruction) =>
            _il.InsertBefore(_originalFirstInstruction, instruction);
    }
}