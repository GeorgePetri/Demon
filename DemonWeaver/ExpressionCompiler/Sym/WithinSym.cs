using DemonWeaver.ExpressionCompiler.Sym.Interface;

namespace DemonWeaver.ExpressionCompiler.Sym
{
    public class WithinSym : ISym
    {
        //todo idea: remove value from here use another string type instead
        public WithinSym(string value) => Value = value;

        public string Value { get; }
    }
}