using Demon.Fody.PointcutExpression.Token.Interface;

namespace Demon.Fody.PointcutExpression.Token
{
    public class PointcutToken : IToken
    {
        public void Accept(ITokenVisitor visitor) => visitor.Visit(this);
    }
}