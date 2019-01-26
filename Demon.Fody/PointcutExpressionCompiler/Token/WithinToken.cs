using Demon.Fody.PointcutExpressionCompiler.Token.Interface;

namespace Demon.Fody.PointcutExpressionCompiler.Token
{
    public class WithinToken : IToken
    {
        public WithinToken(string s) => String = s;

        public string String { get;  }
        
        public void Accept(ITokenVisitor visitor) => visitor.Visit(this);
    }
}