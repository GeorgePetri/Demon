using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Demon.Fody.PointcutExpression.Token;
using Fody;
using Mono.Cecil;

namespace Demon.Fody.PointcutExpression
{
    //todo do validation
    public class CodeGenVisitor : ITokenVisitor
    {
        private static readonly MethodInfo RegexIsMatchMethod = typeof(Regex).GetMethod(nameof(Regex.IsMatch), new[] {typeof(string)});

        private readonly ParameterExpression _parameter = Expression.Parameter(typeof(MethodDefinition));

        private readonly Stack<Expression> _stack = new Stack<Expression>();

        public void Visit(AndAlsoToken _) =>
            HandleBinaryOperation(Expression.AndAlso, "\"&&\" must be preceded by two operations");

        public void Visit(ExecutionToken execution)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(NotToken _)
        {
            Expression previous;
            try
            {
                previous = _stack.Pop();
            }
            catch (InvalidOperationException)
            {
                throw new WeavingException("\"!\" can not be the first operation");
            }

            var not = Expression.Not(previous);

            _stack.Push(not);
        }

        public void Visit(OrElseToken _) =>
            HandleBinaryOperation(Expression.OrElse, "\"||\" must be preceded by two operations");

        public void Visit(PointcutToken pointcut)
        {
            throw new System.NotImplementedException();
        }

        //todo validate
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

        private void HandleBinaryOperation(Func<Expression, Expression, Expression> func, string exceptionText)
        {
            Expression popped1;
            Expression popped2;
            try
            {
                popped1 = _stack.Pop();
                popped2 = _stack.Pop();
            }
            catch (InvalidOperationException)
            {
                throw new WeavingException(exceptionText);
            }

            var expression = func(popped2, popped1);

            _stack.Push(expression);
        }
    }
}