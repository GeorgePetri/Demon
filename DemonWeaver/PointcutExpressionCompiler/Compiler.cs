using System;
using Mono.Cecil;

namespace DemonWeaver.PointcutExpressionCompiler
{
    public class Compiler
    {
        readonly PointcutExpression _expression;
        readonly PointcutContext _pointcutContext;

        public Compiler(PointcutExpression expression, PointcutContext pointcutContext) =>
            (_expression, _pointcutContext) = (expression, pointcutContext);

        public static Func<MethodDefinition, bool> Compile(PointcutExpression expression, PointcutContext pointcutContext) =>
            new Compiler(expression, pointcutContext).Compile();

        //todo impl use DefiningMethod
        public Func<MethodDefinition, bool> Compile()
        {
            if(string.IsNullOrWhiteSpace(_expression.String))
                throw new WeavingException("Expression is empty.");
            
            var tokens = Lexer.Analyse(_expression.String);

            var expresionVisitor = new CodeGenVisitor(_pointcutContext);

            foreach (var token in tokens) 
                token.Accept(expresionVisitor);

            return expresionVisitor.GetExpression();
        }
    }
}