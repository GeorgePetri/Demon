using System;
using Mono.Cecil;
using Environment = DemonWeaver.ExpressionCompiler.Data.Environment;
using PointcutExpression = DemonWeaver.PointcutExpressionCompiler.Data.PointcutExpression;

namespace DemonWeaver.ExpressionCompiler
{
    public class Compiler
    {
        readonly PointcutExpression _expression;
        readonly Environment _environment;

        public Compiler(PointcutExpression expression, Environment environment) =>
            (_expression, _environment) = (expression, environment);

        public static Func<MethodDefinition, bool> Compile(PointcutExpression expression, Environment environment) =>
            new Compiler(expression, environment).Compile();

        public Func<MethodDefinition, bool> Compile()
        {
            return default;
        }
    }
}