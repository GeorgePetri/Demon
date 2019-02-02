using DemonWeaver.PointcutExpressionCompiler.Token.Interface;

namespace DemonWeaver.PointcutExpressionCompiler.Token
{
    public class NotToken : IToken
    {
        public void Accept(ITokenVisitor visitor) => visitor.Visit(this);
    }
}