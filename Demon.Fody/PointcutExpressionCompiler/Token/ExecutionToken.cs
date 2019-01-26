using Demon.Fody.PointcutExpressionCompiler.Token.Interface;

namespace Demon.Fody.PointcutExpressionCompiler.Token
{
    public class ExecutionToken : IToken
    {
        public void Accept(ITokenVisitor visitor) => visitor.Visit(this);
    }
}