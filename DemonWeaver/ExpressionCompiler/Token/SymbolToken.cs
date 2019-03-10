using DemonWeaver.ExpressionCompiler.Token.Interface;

namespace DemonWeaver.ExpressionCompiler.Token
{
    //todo is name right?
    public class SymbolToken : IToken
    {
        public SymbolToken(string value) => Value = value;

        public string Value { get; }
    }
}