using System;
using System.Linq;
using DemonWeaver.ExpressionCompiler.Data;
using Mono.Cecil;
using Environment = DemonWeaver.ExpressionCompiler.Data.Environment;

namespace DemonWeaver.ExpressionCompiler
{
    //todo should functions with no argument require parentheses?
    public class Compiler
    {
        readonly PointcutExpression _expression;
        readonly Environment _environment;

        public Compiler(PointcutExpression expression, Environment environment) =>
            (_expression, _environment) = (expression, environment);

        public static Func<MethodDefinition, bool> Compile(PointcutExpression expression, Environment environment) =>
            new Compiler(expression, environment).Compile();

        //todo the code can e ordered weirdly for complex expressions
        public Func<MethodDefinition, bool> Compile()
        {
            var tokens = Lexer.AnalyseExpression(_expression.String).ToList();

            var syms = Parser.Parse(tokens);

            return CodeGenerator.Generate(syms, _expression.DefiningMethod, _environment);
        }
    }
}