using Mono.Cecil;
using Mono.Cecil.Cil;

namespace DemonWeaver
{
    //todo impl and don't copy paste
    //todo either make another class with common or merge this and BeforeMethodWeaver
    public class AroundMethodWeaver
    {
        readonly TypeWeaver _typeWeaver;
        readonly MethodDefinition _target;
        readonly MethodReference _advice;
        readonly FieldDefinition _adviceField;
        readonly ILProcessor _il;
        readonly Instruction _originalFirstInstruction;

        public AroundMethodWeaver(TypeWeaver typeWeaver, MethodDefinition target, MethodReference advice, FieldDefinition adviceField)
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
        }
    }
}