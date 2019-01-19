namespace Demon.Fody.PointcutExpression
{
    public class Compiler
    {
        private readonly string _expression;

        public Compiler(string expression)
        {
            _expression = expression;
        }

        public void Compile()
        {
            var expresionVisitor = new ExpressionVisitor();
            var tokens = Lexer.Analyse(_expression);

            foreach (var token in tokens)
            {
                token.Accept(expresionVisitor);
            }
        }
    }
}