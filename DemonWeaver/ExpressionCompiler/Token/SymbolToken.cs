using DemonWeaver.ExpressionCompiler.Token.Interface;

namespace DemonWeaver.ExpressionCompiler.Token
{
    //todo is name right?
    //todo replace usages with strings
    public class SymbolToken : IToken
    {
        public SymbolToken(string value) => Value = value;

        public string Value { get; }
    }
}