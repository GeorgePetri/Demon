using DemonWeaver.PointcutExpressionCompiler.Token.Interface;

namespace DemonWeaver.PointcutExpressionCompiler.Token
{
    public class ArgsToken : IToken
    {
        public ArgsToken(string s) => String = s;

        public string String { get; }
        
        public void Accept(ITokenVisitor visitor) => visitor.Visit(this);
    }
}