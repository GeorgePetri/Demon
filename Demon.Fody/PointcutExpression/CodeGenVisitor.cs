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
    //todo cleanup the class is very messy, also, do all string manipulation either here or elsewhere
    public class CodeGenVisitor : ITokenVisitor
    {
        static readonly MethodInfo RegexIsMatchMethod = typeof(Regex).GetMethod(nameof(Regex.IsMatch), new[] {typeof(string)});

        readonly ParameterExpression _parameter = Expression.Parameter(typeof(MethodDefinition));

        readonly Stack<Expression> _stack = new Stack<Expression>();

        readonly PointcutContext _pointcutContext;

        public CodeGenVisitor(PointcutContext pointcutContext) => _pointcutContext = pointcutContext;

        public void Visit(AndAlsoToken _) =>
            HandleBinaryOperation(Expression.AndAlso, "\"&&\" must be preceded by two operations.");

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
                throw new WeavingException("\"!\" can not be the first operation.");
            }

            var not = Expression.Not(previous);

            _stack.Push(not);
        }

        public void Visit(OrElseToken _) =>
            HandleBinaryOperation(Expression.OrElse, "\"||\" must be preceded by two operations.");

        public void Visit(PointcutToken pointcut)
        {
            var value = pointcut.String;

            var withoutParentheses = value.Substring(0, value.Length - 2);

            var pointcutFunc = _pointcutContext.GetResolved(withoutParentheses);

            var invoke = Expression.Invoke(Expression.Constant(pointcutFunc), _parameter);

            _stack.Push(invoke);
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
            if(_stack.Count != 1)
                throw new WeavingException(@"Invalid expression is there a missing && or ||?"); 

            var body = _stack.Pop();
            
            var expression = Expression.Lambda<Func<MethodDefinition, bool>>(body, _parameter);

            return expression.Compile();
        }

        //todo use this only once per visitor parse
        //todo cache reflective calls
        MethodCallExpression GetFullName()
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

        void HandleBinaryOperation(Func<Expression, Expression, Expression> func, string exceptionText)
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