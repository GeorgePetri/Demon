using Demon.Fody.PointcutExpression.Token.Interface;

namespace Demon.Fody.PointcutExpression.Token
{
    public class WithinToken : IToken
    {
        public WithinToken(string s) => String = s;

        public string String { get;  }
        
        public void Accept(ITokenVisitor visitor) => visitor.Visit(this);
    }
}