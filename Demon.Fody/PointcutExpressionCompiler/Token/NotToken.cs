using Demon.Fody.PointcutExpressionCompiler.Token.Interface;

namespace Demon.Fody.PointcutExpressionCompiler.Token
{
    public class NotToken : IToken
    {
        public void Accept(ITokenVisitor visitor) => visitor.Visit(this);
    }
}