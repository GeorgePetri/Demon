using System;
using System.Linq.Expressions;
using Demon.Fody.PointcutExpression.Token;
using Mono.Cecil;

namespace Demon.Fody.PointcutExpression
{
    public class ExpressionVisitor : ITokenVisitor
    {
        private Expression _expression;

        public void Visit(AndAlsoToken andAlso)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(ExecutionToken execution)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(NotToken not)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(OrElseToken orElse)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(PointcutToken pointcut)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(WithinToken withinToken)
        {
            throw new System.NotImplementedException();
        }

        public Func<MethodDefinition, bool> GetExpression()
        {
            var parameter = Expression.Parameter(typeof(MethodDefinition));

            return Expression.Lambda<Func<MethodDefinition, bool>>(_expression, parameter)
                .Compile();
        }
    }
}