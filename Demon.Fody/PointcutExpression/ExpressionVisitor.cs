using System;
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
            var regex = RegexFactory.TryProcessWithin(withinToken.String);

            var regexInstance = Expression.Constant(regex);

            //todo don't equals, combine expression or initialize;
            _expression = Expression.Call(regexInstance, RegexIsMatchMethod, GetFullName());
        }

        public Func<MethodDefinition, bool> GetExpression()
        {
            var expression = Expression.Lambda<Func<MethodDefinition, bool>>(_expression, _parameter);

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
            var format = Expression.Constant("{1}.{2}");

            var formated = Expression.Call(formatMethod, format, declaringFullName, name);

            return formated;
        }
    }
}