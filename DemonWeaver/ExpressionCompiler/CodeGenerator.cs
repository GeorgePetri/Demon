using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using DemonWeaver.ExpressionCompiler.Helpers;
using DemonWeaver.ExpressionCompiler.Sym;
using DemonWeaver.ExpressionCompiler.Sym.Interface;
using Mono.Cecil;

namespace DemonWeaver.ExpressionCompiler
{
    public class CodeGenerator
    {
        readonly Stack<ISym> _syms;
        readonly Stack<Expression> _stack = new Stack<Expression>();

        public CodeGenerator(Stack<ISym> syms) => _syms = syms;

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
            switch (_syms.Pop())
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
                case SymbolSym _:
                    Symbol();
                    break;
                case WithinSym _:
                    Within();
                    break;
            }
        }

        void AndAlso()
        {
            throw new System.NotImplementedException();
        }

        void Not()
        {
            throw new System.NotImplementedException();
        }

        void OrElse()
        {
            throw new System.NotImplementedException();
        }

        void String(StringSym stringSym) => Push(Expression.Constant(stringSym.Value));

        void Symbol()
        {
            throw new System.NotImplementedException();
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
    }
}