using System;
using Mono.Cecil;

namespace Demon.Fody.PointcutExpression
{
    public class Compiler
    {
        readonly string _expression;

        public Compiler(string expression)
        {
            _expression = expression;
        }

        public Func<MethodDefinition, bool> Compile()
        {
            var tokens = Lexer.Analyse(_expression);

            var expresionVisitor = new CodeGenVisitor();
            
            foreach (var token in tokens)
            {
                token.Accept(expresionVisitor);
            }

            return expresionVisitor.GetExpression();
        }
    }
}