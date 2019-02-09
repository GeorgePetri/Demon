using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using DemonWeaver.PointcutExpressionCompiler.Data;
using DemonWeaver.PointcutExpressionCompiler.Token;
using Mono.Cecil;
using Expressions = DemonWeaver.PointcutExpressionCompiler.LinqExpressionFactory;

namespace DemonWeaver.PointcutExpressionCompiler
{
    public class CodeGenVisitor : ITokenVisitor
    {
        static readonly MethodInfo RegexIsMatchMethod = typeof(Regex).GetMethod(nameof(Regex.IsMatch), new[] {typeof(string)});

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

            if (!strings.Any())
                _stack.Push(Expression.Not(Expressions.HasParameters));
            else
            {
                var toBeBound = new HashSet<(string name, TypeReference type)>();
                var argCountMustBeAtLeast = 0;
                var argCountHasUpperBound = true;
                //todo compiler should return metadata about bound args besides the func, make sure it works with pointcuts
                foreach (var argument in strings)
                {
                    argCountMustBeAtLeast++;

                    if (argument == @"**")
                    {
                        argCountHasUpperBound = false;
                        continue;
                    }

                    if (argument == @"*")
                        continue;

                    var parameterDefinition = _definingMethod
                                                  .Parameters
                                                  .FirstOrDefault(p => p.Name == argument)
                                              ?? throw new WeavingException($"{argument} in {args.String} is not found in the defining method");

                    toBeBound.Add((argument, parameterDefinition.ParameterType));
                }

                _stack.Push(argCountHasUpperBound
                    ? Expressions.TargetParameterEqual(argCountMustBeAtLeast)
                    : Expressions.TargetParameterGreaterThanOrEqual(argCountMustBeAtLeast));

                if (toBeBound.Any())
                {
                    var previous = _stack.Pop();

                    var targetHasParametersOfType = Expressions.TargetHasParametersOfType(toBeBound.Select(t => t.type));

                    _stack.Push(Expression.AndAlso(previous, targetHasParametersOfType));
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

            var invoke = Expression.Invoke(Expression.Constant(pointcutFunc), Expressions.Target);

            _stack.Push(invoke);
        }

        public void Visit(WithinToken withinToken)
        {
            var regex = TokenValueParser.Process(withinToken);

            var regexInstance = Expression.Constant(new Regex(regex, RegexOptions.Compiled));

            _stack.Push(Expression.Call(regexInstance, RegexIsMatchMethod, Expressions.TargetFullName));
        }

        public Func<MethodDefinition, bool> GetExpression()
        {
            if (_stack.Count != 1)
                throw new WeavingException(@"Invalid expression.Is there a missing && or ||?");

            var body = _stack.Pop();

            var expression = Expression.Lambda<Func<MethodDefinition, bool>>(body, Expressions.Target);

            return expression.Compile();
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