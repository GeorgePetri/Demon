using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using DemonWeaver.ExpressionCompiler.Helpers;
using DemonWeaver.ExpressionCompiler.Sym;
using DemonWeaver.ExpressionCompiler.Sym.Interface;
using Mono.Cecil;
using Environment = DemonWeaver.ExpressionCompiler.Data.Environment;

namespace DemonWeaver.ExpressionCompiler
{
    public class CodeGenerator
    {
        readonly List<ISym> _syms;
        readonly MethodDefinition _definingMethod;
        readonly Environment _environment;

        readonly Stack<Expression> _stack = new Stack<Expression>();

        public CodeGenerator(List<ISym> syms, MethodDefinition definingMethod, Environment environment) =>
            (_syms, _definingMethod, _environment) = (syms, definingMethod, environment);

        public static Func<MethodDefinition, bool> Generate(List<ISym> syms, MethodDefinition definingMethod, Environment environment) =>
            new CodeGenerator(syms, definingMethod, environment).Generate();

        public Func<MethodDefinition, bool> Generate()
        {
            while (_syms.Any())
                GenerateNext();

            var body = Pop();

            var expression = Expression.Lambda<Func<MethodDefinition, bool>>(body, LinqExpressions.Target);

            return expression.Compile();
        }

        void GenerateNext()
        {
            var value = _syms.First();

            _syms.RemoveAt(0);

            switch (value)
            {
                case AndAlsoSym _:
                    AndAlso();
                    break;
                case NotSym _:
                    Not();
                    break;
                case OrElseSym _:
                    OrElse();
                    break;
                case StringSym stringSym:
                    String(stringSym);
                    break;
                case SymbolSym symbol:
                    Symbol(symbol.Value);
                    break;
                case WithinSym _:
                    Within();
                    break;
            }
        }

        void AndAlso() => HandleBinaryOperation(Expression.AndAlso);

        void Not()
        {
            var previous = Pop();

            var not = Expression.Not(previous);

            Push(not);
        }

        void OrElse() => HandleBinaryOperation(Expression.OrElse);

        void String(StringSym stringSym) => Push(Expression.Constant(stringSym.Value));

        void Symbol(string value)
        {
            var func = _environment.Resolve(value);

            Push(Expression.Invoke(Expression.Constant(func), LinqExpressions.Target));
        }

        void Within()
        {
            var withinParameter = GetString(Pop());

            var regexInstance = Expression.Constant(CreateWithinPredicateRegex(withinParameter));

            Push(Expression.Call(regexInstance, Methods.RegexIsMatchMethod, LinqExpressions.TargetFullName));
        }

        static Regex CreateWithinPredicateRegex(string value)
        {
            var escapeDot = value.Replace(".", @"\.");

            var replacedDoubleStar = escapeDot.Replace("**", @"[\w.]+");

            var replacedSingleStar = replacedDoubleStar.Replace("*", @"[\w]+");

            var withEndString = $"^{replacedSingleStar}$";

            return new Regex(withEndString, RegexOptions.Compiled);
        }

        void Push(Expression expression) => _stack.Push(expression);

        Expression Pop() => _stack.Pop();

        static string GetString(Expression expression) => (string) ((ConstantExpression) expression).Value;

        void HandleBinaryOperation(Func<Expression, Expression, Expression> func) =>
            Push(func(Pop(), Pop()));
    }
}