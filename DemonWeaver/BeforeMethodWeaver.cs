using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace DemonWeaver
{
    public class BeforeMethodWeaver
    {
        readonly TypeWeaver _typeWeaver;
        readonly MethodDefinition _target;
        readonly MethodReference _advice;
        readonly FieldDefinition _adviceField;
        readonly ILProcessor _il;
        readonly Instruction _originalFirstInstruction;

        public BeforeMethodWeaver(TypeWeaver typeWeaver, MethodDefinition target, MethodReference advice, FieldDefinition adviceField)
        {
            _typeWeaver = typeWeaver;
            _target = target;
            _advice = advice;
            _adviceField = adviceField;
            _il = target.Body.GetILProcessor();
            _originalFirstInstruction = target.Body.Instructions[0];
        }

        public void Weave()
        {
            InsertLoadAspectIfNeeded();
            InsertLoadAdviceParametersIfNeeded();

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

        void InsertLoadAdviceParametersIfNeeded()
        {
            if (!_advice.HasParameters)
                return;

            foreach (var parameter in _advice.Parameters)
            {
                var parameterToLoad = _target.Parameters.FirstOrDefault(p => p.ParameterType == parameter.ParameterType);
                if (parameterToLoad != null)
                {
                    var loadParameter = _il.GetEfficientLoadInstruction(parameterToLoad);
                    InsertBeforeOriginalFirst(loadParameter);
                }
                else if (parameter.ParameterType.FullName == "Demon.JoinPoint.TypeJoinPoint")
                {
                    var typeJoinPointField = _typeWeaver.WeaveTypeJoinPointField(_target, parameter.ParameterType);
                    var loadTypeJoinPoint = _il.Create(OpCodes.Ldsfld, typeJoinPointField);
                    InsertBeforeOriginalFirst(loadTypeJoinPoint);
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