using DemonWeaver.ExpressionCompiler.Token.Interface;

namespace DemonWeaver.ExpressionCompiler.Token
{
    public class StringToken : IToken
    {
        public StringToken(string value) => Value = value;

        public string Value { get; }
    }
}