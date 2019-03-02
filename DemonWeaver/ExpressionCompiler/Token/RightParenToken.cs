using DemonWeaver.ExpressionCompiler.Token.Interface;

namespace DemonWeaver.ExpressionCompiler.Token
{
    public class RightParenToken : IToken
    {
        public void Accept(ITokenVisitor visitor) => visitor.Visit(this);
    }
}