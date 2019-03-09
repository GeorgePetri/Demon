using DemonWeaver.ExpressionCompiler.Sym.Interface;

namespace DemonWeaver.ExpressionCompiler.Sym
{
    //todo really bad naming
    public class SymbolSym : ISym
    {
        public SymbolSym(string value) => Value = value;

        public string Value { get; }
    }
}