using Mono.Cecil;

namespace DemonWeaver.PointcutExpressionCompiler
{
    public class PointcutExpression
    {
        public PointcutExpression(string expression, MethodDefinition definingMethod) =>
            (String, DefiningMethod) = (expression, definingMethod);

        public string String { get; }

        public MethodDefinition DefiningMethod { get; }
    }
}