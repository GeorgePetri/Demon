using DemonWeaver.ExpressionCompiler.Data;
using PointcutExpression = DemonWeaver.PointcutExpressionCompiler.Data.PointcutExpression;

namespace DemonWeaver.ExpressionCompiler
{
    public class Compiler
    {
        readonly PointcutExpression _expression;
        readonly Environment _environment;

        public Compiler(PointcutExpression expression, Environment environment) =>
            (_expression, _environment) = (expression, environment);
    }
}