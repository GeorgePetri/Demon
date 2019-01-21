using System;
using Mono.Cecil;

namespace Demon.Fody.PointcutExpression
{
    public class Compiler
    {
        readonly string _expression;
        readonly PointcutContext _pointcutContext;

        public Compiler(string expression, PointcutContext pointcutContext) => 
            (_expression, _pointcutContext) = (expression, pointcutContext);

        public Func<MethodDefinition, bool> Compile()
        {
            var tokens = Lexer.Analyse(_expression);

            var expresionVisitor = new CodeGenVisitor(_pointcutContext);

            foreach (var token in tokens)
            {
                token.Accept(expresionVisitor);
            }

            return expresionVisitor.GetExpression();
        }
    }
}