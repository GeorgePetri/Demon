using System;
using Fody;
using Mono.Cecil;

namespace Demon.Fody.PointcutExpressionCompiler
{
    public class Compiler
    {
        readonly string _expression;
        readonly PointcutContext _pointcutContext;

        public Compiler(string expression, PointcutContext pointcutContext) =>
            (_expression, _pointcutContext) = (expression, pointcutContext);

        public static Func<MethodDefinition, bool> Compile(string expression, PointcutContext pointcutContext) =>
            new Compiler(expression, pointcutContext).Compile();

        public Func<MethodDefinition, bool> Compile()
        {
            if(string.IsNullOrWhiteSpace(_expression))
                throw new WeavingException("Expression is empty.");
            
            var tokens = Lexer.Analyse(_expression);

            var expresionVisitor = new CodeGenVisitor(_pointcutContext);

            foreach (var token in tokens) 
                token.Accept(expresionVisitor);

            return expresionVisitor.GetExpression();
        }
    }
}