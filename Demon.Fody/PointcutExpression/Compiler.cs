using System;
using Mono.Cecil;

namespace Demon.Fody.PointcutExpression
{
    public class Compiler
    {
        private readonly string _expression;

        public Compiler(string expression)
        {
            _expression = expression;
        }

        public Func<MethodDefinition, bool> Compile()
        {
            var tokens = Lexer.Analyse(_expression);

            //todo do validation here or in separate visitor
            var expresionVisitor = new ExpressionVisitor();
            
            foreach (var token in tokens)
            {
                token.Accept(expresionVisitor);
            }

            return expresionVisitor.GetExpression();
        }
    }
}