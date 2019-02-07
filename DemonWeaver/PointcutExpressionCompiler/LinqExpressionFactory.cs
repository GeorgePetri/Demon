using System.Linq.Expressions;
using Mono.Cecil;
using Mono.Collections.Generic;

// ReSharper disable AssignNullToNotNullAttribute
namespace DemonWeaver.PointcutExpressionCompiler
{
    public static class LinqExpressionFactory
    {
        public static ParameterExpression Target { get; } = Expression.Parameter(typeof(MethodDefinition));

        public static MethodCallExpression TargetFullName { get; } = CreateTargetFullName();

        public static MemberExpression TargetParameterCount { get; } = CreateTargetParameterCount();

        public static BinaryExpression TargetParameterEqual(int value) => 
            Expression.Equal(TargetParameterCount, Expression.Constant(value));
        public static BinaryExpression TargetParameterGreaterThanOrEqual(int value) => 
            Expression.GreaterThanOrEqual(TargetParameterCount, Expression.Constant(value));

        static MethodCallExpression CreateTargetFullName()
        {
            var name = Expression.Property(Target, typeof(MethodDefinition).GetProperty(nameof(MethodDefinition.Name)));

            var declaringType = Expression.Property(
                Target,
                typeof(MethodDefinition)
                    .GetProperty(nameof(MethodDefinition.DeclaringType), typeof(TypeDefinition)));

            var declaringFullName = Expression.Property(declaringType, typeof(TypeDefinition).GetProperty(nameof(TypeDefinition.FullName)));

            var formatMethod = typeof(string).GetMethod(nameof(string.Format), new[] {typeof(string), typeof(object), typeof(object)});

            var stringFormat = Expression.Constant("{0}.{1}");

            return Expression.Call(formatMethod, stringFormat, declaringFullName, name);
        }

        static MemberExpression CreateTargetParameterCount()
        {
            var parameters = Expression.Property(Target, typeof(MethodDefinition).GetProperty(nameof(MethodDefinition.Parameters)));
            var count = Expression.Property(parameters, typeof(Collection<ParameterDefinition>).GetProperty(nameof(Collection<ParameterDefinition>.Count)));

            return count;
        }
    }
}