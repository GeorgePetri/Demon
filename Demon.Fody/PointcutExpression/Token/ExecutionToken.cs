using Demon.Fody.PointcutExpression.Token.Interface;

namespace Demon.Fody.PointcutExpression.Token
{
    public class ExecutionToken : IToken
    {
        public T Accept<T>(ITokenVisitor<T> visitor) => visitor.Visit(this);
    }
}