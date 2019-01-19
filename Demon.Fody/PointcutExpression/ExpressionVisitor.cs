using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Demon.Fody.PointcutExpression.Token;
using Mono.Cecil;

namespace Demon.Fody.PointcutExpression
{
    //todo rename
    public class ExpressionVisitor : ITokenVisitor
    {
        private static readonly MethodInfo RegexIsMatchMethod = typeof(Regex).GetMethod(nameof(Regex.IsMatch), new[] {typeof(string)});

        private readonly ParameterExpression _parameter = Expression.Parameter(typeof(MethodDefinition));
        
        private readonly Stack<Expression> _stack = new Stack<Expression>();

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
            var regex = RegexFactory.TryProcessWithin(withinToken.String);

            var regexInstance = Expression.Constant(regex);

            _stack.Push(Expression.Call(regexInstance, RegexIsMatchMethod, GetFullName()));
        }

        public Func<MethodDefinition, bool> GetExpression()
        {
            var body = _stack.Pop();
            
            var expression = Expression.Lambda<Func<MethodDefinition, bool>>(body, _parameter);

            return expression.Compile();
        }

        //todo use this only once per visitor parse
        //todo cache reflective calls
        private MethodCallExpression GetFullName()
        {
            var name = Expression.Property(_parameter, typeof(MethodDefinition).GetProperty(nameof(MethodDefinition.Name)));

            var declaringType = Expression.Property(_parameter, typeof(MethodDefinition)
                .GetProperty(nameof(MethodDefinition.DeclaringType), typeof(TypeDefinition)));

            var declaringFullName = Expression.Property(declaringType, typeof(TypeDefinition).GetProperty(nameof(TypeDefinition.FullName)));

            var formatMethod = typeof(string).GetMethod(nameof(string.Format), new Type[] {typeof(string), typeof(object), typeof(object)});
            var format = Expression.Constant("{0}.{1}");

            var formated = Expression.Call(formatMethod, format, declaringFullName, name);

            return formated;
        }
    }
}