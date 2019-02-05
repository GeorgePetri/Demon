using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using DemonWeaver.PointcutExpressionCompiler.Token;
using Mono.Cecil;
using Mono.Collections.Generic;

namespace DemonWeaver.PointcutExpressionCompiler
{
    //todo do validation
    //todo cleanup the class is very messy, also, do all string manipulation either here or elsewhere
    public class CodeGenVisitor : ITokenVisitor
    {
        //todo extract these in separate class once critical mass is reached
        static readonly MethodInfo RegexIsMatchMethod = typeof(Regex).GetMethod(nameof(Regex.IsMatch), new[] {typeof(string)});
        static readonly ParameterExpression Parameter = Expression.Parameter(typeof(MethodDefinition));
        static readonly MethodCallExpression GetFullName = CreateGetFullNameExpression();
        static readonly MemberExpression ParameterCount = CreateParameterCountExpression();

        static readonly Regex CanBeFullname = new Regex(@"^\w*$", RegexOptions.Compiled);

        readonly Stack<Expression> _stack = new Stack<Expression>();
        readonly MethodDefinition _definingMethod;
        readonly PointcutContext _pointcutContext;

        public CodeGenVisitor(MethodDefinition definingMethod, PointcutContext pointcutContext) =>
            (_definingMethod, _pointcutContext) = (definingMethod, pointcutContext);

        public void Visit(AndAlsoToken _) =>
            HandleBinaryOperation(Expression.AndAlso, "\"&&\" must be preceded by two operations.");

        public void Visit(ArgsToken args)
        {
            var strings = TokenValueParser.Process(args);

            //todo cleanup, use ANy instead of count on expression
            if (!strings.Any())
            {
                var zero = Expression.Constant(0);

                var equals = Expression.Equal(ParameterCount, zero);

                _stack.Push(equals);
            }
            else
            {
                //todo compiler should return metadata about bound args besides the func
                //todo move to own method
                foreach (var argument in strings)
                {
                    if (!CanBeFullname.IsMatch(argument))
                        throw new WeavingException($"Invalid argument name:{argument} in {args.String}");

                    if (argument == @"**")
                        throw new NotImplementedException();

                    if (argument == @"*")
                        throw new NotImplementedException();

                    var parameterDefinition = _definingMethod
                                                  .Parameters
                                                  .FirstOrDefault(p => p.Name == argument)
                                              ?? throw new WeavingException($"{argument} in {args.String} is not found in the defining method");
                }
            }
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
                throw new WeavingException("\"!\" can not be the first symbol.");
            }

            var not = Expression.Not(previous);

            _stack.Push(not);
        }

        public void Visit(OrElseToken _) =>
            HandleBinaryOperation(Expression.OrElse, "\"||\" must be preceded by two symbol.");

        public void Visit(PointcutToken pointcut)
        {
            var name = TokenValueParser.Process(pointcut);

            var pointcutFunc = _pointcutContext.GetResolved(name);

            var invoke = Expression.Invoke(Expression.Constant(pointcutFunc), Parameter);

            _stack.Push(invoke);
        }

        //todo validate
        public void Visit(WithinToken withinToken)
        {
            var regex = RegexFactory.TryProcessWithin(withinToken.String);

            var regexInstance = Expression.Constant(regex);

            _stack.Push(Expression.Call(regexInstance, RegexIsMatchMethod, GetFullName));
        }

        public Func<MethodDefinition, bool> GetExpression()
        {
            if (_stack.Count != 1)
                throw new WeavingException(@"Invalid expression.Is there a missing && or ||?");

            var body = _stack.Pop();

            var expression = Expression.Lambda<Func<MethodDefinition, bool>>(body, Parameter);

            return expression.Compile();
        }

        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        static MethodCallExpression CreateGetFullNameExpression()
        {
            var name = Expression.Property(Parameter, typeof(MethodDefinition).GetProperty(nameof(MethodDefinition.Name)));

            var declaringType = Expression.Property(
                Parameter,
                typeof(MethodDefinition)
                    .GetProperty(nameof(MethodDefinition.DeclaringType), typeof(TypeDefinition)));

            var declaringFullName = Expression.Property(declaringType, typeof(TypeDefinition).GetProperty(nameof(TypeDefinition.FullName)));

            var formatMethod = typeof(string).GetMethod(nameof(string.Format), new[] {typeof(string), typeof(object), typeof(object)});

            var stringFormat = Expression.Constant("{0}.{1}");

            return Expression.Call(formatMethod, stringFormat, declaringFullName, name);
        }

        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        static MemberExpression CreateParameterCountExpression()
        {
            var parameters = Expression.Property(Parameter, typeof(MethodDefinition).GetProperty(nameof(MethodDefinition.Parameters)));
            var count = Expression.Property(parameters, typeof(Collection<ParameterDefinition>).GetProperty(nameof(Collection<ParameterDefinition>.Count)));

            return count;
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