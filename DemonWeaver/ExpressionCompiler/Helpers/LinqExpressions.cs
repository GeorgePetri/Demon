using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Mono.Cecil;

// ReSharper disable AssignNullToNotNullAttribute
namespace DemonWeaver.ExpressionCompiler.Helpers
{
    public static class LinqExpressions
    {
        public static ParameterExpression Target { get; } = Expression.Parameter(typeof(MethodDefinition), "m");

        public static MethodCallExpression TargetFullName { get; } = CreateTargetFullName();

        public static MemberExpression HasParameters { get; } = Expression.Property(Target, Methods.MethodDefinition_HasParameters);

        public static MemberExpression TargetParameterCount { get; } = CreateTargetParameterCount();

        static Expression<Func<TypeReference, bool>> TargetHasParametersOfTypeBody { get; } = CreateTargetHasParametersOfTypeBody();

        public static BinaryExpression TargetParameterEqual(int value) =>
            Expression.Equal(TargetParameterCount, Expression.Constant(value));

        public static BinaryExpression TargetParameterGreaterThanOrEqual(int value) =>
            Expression.GreaterThanOrEqual(TargetParameterCount, Expression.Constant(value));

        /// <summary>
        /// Resulting expression looks like <c>types.All(t => m.Parameters.Any(p => p.ParameterType.FullName == t.FullName))</c>;
        /// </summary>
        public static MethodCallExpression TargetHasParametersOfType(IEnumerable<TypeReference> types) =>
            Expression.Call(
                Methods.IEnumerableOfTypeReference_All,
                Expression.Constant(types),
                TargetHasParametersOfTypeBody);

        static Expression<Func<TypeReference, bool>> CreateTargetHasParametersOfTypeBody()
        {
            var typeParameter = Expression.Parameter(typeof(TypeReference), "t"); //t

            var parameterParameter = Expression.Parameter(typeof(ParameterDefinition), "p"); //p

            var parameterParameterType = Expression.Property(
                parameterParameter,
                Methods.ParameterDefinition_ParameterType); //p.ParameterType

            var parameterParameterTypeFullName = Expression.Property(
                parameterParameterType,
                Methods.TypeReference_FullName); //p.ParameterType.FullName

            var typeParameterFullName = Expression.Property(
                typeParameter,
                Methods.TypeReference_FullName); //t.FullName

            var parameterTypeEqual = Expression.Equal(parameterParameterTypeFullName, typeParameterFullName); //p.ParameterType.FullName == t.FullName

            var parametersAnyLambda = Expression.Lambda<Func<ParameterDefinition, bool>>(parameterTypeEqual, parameterParameter); //p => p.ParameterType.FullName == t.FullName

            var targetParameters = Expression.Property(
                Target,
                Methods.MethodDefinition_Parameters); //m.Parameters

            var targetParametersAny = Expression.Call(
                Methods.IEnumerableOfParameterDefinition_Any,
                targetParameters,
                parametersAnyLambda); //m.Parameters.Any(p => p.ParameterType.FullName == t.FullName)

            return Expression.Lambda<Func<TypeReference, bool>>(targetParametersAny, typeParameter); //t => m.Parameters.Any(p => p.ParameterType.FullName == t.FullName)
        }

        static MethodCallExpression CreateTargetFullName()
        {
            var name = Expression.Property(Target, Methods.MethodDefinition_Name);

            var declaringType = Expression.Property(Target, Methods.MethodDefinition_DeclaringType);

            var declaringFullName = Expression.Property(declaringType, Methods.TypeDefinition_FullName);

            var stringFormat = Expression.Constant("{0}.{1}");

            return Expression.Call(Methods.String_Format, stringFormat, declaringFullName, name);
        }

        static MemberExpression CreateTargetParameterCount()
        {
            var parameters = Expression.Property(Target, Methods.MethodDefinition_Parameters);
            var count = Expression.Property(parameters, Methods.CollectionOfParameterDefinition_Count);

            return count;
        }
    }
}