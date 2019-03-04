using DemonWeaver.ExpressionCompiler.Sym.Interface;

namespace DemonWeaver.ExpressionCompiler.Sym
{
    public class StringSym : ISym
    {
        public StringSym(string value) => Value = value;

        public string Value { get; }
    }
}