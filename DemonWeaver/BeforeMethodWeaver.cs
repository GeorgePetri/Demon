using System.Linq;
using DemonWeaver.Extensions;
using DemonWeaver.IlEmitter;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace DemonWeaver
{
    public class BeforeMethodWeaver
    {
        readonly DemonTypes _demonTypes;
        readonly MethodDefinition _target;
        readonly MethodReference _advice;
        readonly FieldDefinition _adviceField;
        readonly ILProcessor _il;
        readonly Instruction _originalFirstInstruction;
        readonly Emitter _emitter;

        public BeforeMethodWeaver(DemonTypes demonTypes, MethodDefinition target, MethodReference advice, FieldDefinition adviceField)
        {
            _demonTypes = demonTypes;
            _target = target;
            _advice = advice;
            _adviceField = adviceField;
            _il = target.Body.GetILProcessor();
            _originalFirstInstruction = target.Body.Instructions[0];
            _emitter = EmitterFactory.Get(_il, i => _il.InsertBefore(_originalFirstInstruction, i));
        }

        public void Weave()
        {
            InsertLoadAspectIfNeeded();
            InsertLoadAdviceParametersIfNeeded();

            _emitter.Call(_advice);
        }

        void InsertLoadAspectIfNeeded()
        {
            if (_adviceField == null)
                return;

            _emitter.Ldarg_0();
            _emitter.Ldfld(_adviceField);
        }

        //todo this tries to bind everything implicitly, is it good design?
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
                else if (parameter.ParameterType.FullName == DemonTypes.FullNames.TypeJoinPoint)
                {
                    _emitter.Ldstr(_target.Name);
                    _emitter.Ldstr(_target.FullName);
                    _emitter.Newobj(_target.Module.ImportReference(_demonTypes.TypeJoinPointConstructor));
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