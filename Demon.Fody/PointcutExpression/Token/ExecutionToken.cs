using Demon.Fody.PointcutExpression.Token.Interface;

namespace Demon.Fody.PointcutExpression.Token
{
    public class ExecutionToken : IToken
    {
        public void Accept(ITokenVisitor visitor) => visitor.Visit(this);
    }
}