using DemonWeaver.ExpressionCompiler.Sym.Interface;

namespace DemonWeaver.ExpressionCompiler.Sym
{
    public class WithinSym : ISym
    {
        public WithinSym(string value) => Value = value;

        public string Value { get; }
    }
}