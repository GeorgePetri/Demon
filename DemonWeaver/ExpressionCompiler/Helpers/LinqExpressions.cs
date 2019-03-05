using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Mono.Cecil;
using Mono.Collections.Generic;

// ReSharper disable PossibleNullReferenceException
// ReSharper disable AssignNullToNotNullAttribute
namespace DemonWeaver.ExpressionCompiler.Helpers
{
    //todo move reflective calls to methods class
    public static class LinqExpressions
    {
        static readonly MethodInfo IEnumerableOfTypeReferenceAllMethod = typeof(Enumerable)
            .GetMethod(nameof(Enumerable.All))
            .MakeGenericMethod(typeof(TypeReference));

        public static ParameterExpression Target { get; } = Expression.Parameter(typeof(MethodDefinition), "m");

        public static MethodCallExpression TargetFullName { get; } = CreateTargetFullName();

        public static MemberExpression HasParameters { get; } =
            Expression.Property(Target, typeof(MethodDefinition).GetProperty(nameof(MethodDefinition.HasParameters)));

        public static MemberExpression TargetParameterCount { get; } = CreateTargetParameterCount();
        
        static Expression<Func<TypeReference, bool>> TargetHasParametersOfTypeBody  { get; }  = CreateTargetHasParametersOfTypeBody();

        public static BinaryExpression TargetParameterEqual(int value) =>
            Expression.Equal(TargetParameterCount, Expression.Constant(value));

        public static BinaryExpression TargetParameterGreaterThanOrEqual(int value) =>
            Expression.GreaterThanOrEqual(TargetParameterCount, Expression.Constant(value));

        /// <summary>
        /// Resulting expression looks like <c>types.All(t => m.Parameters.Any(p => p.ParameterType.FullName == t.FullName))</c>;
        /// </summary>
        public static MethodCallExpression TargetHasParametersOfType(IEnumerable<TypeReference> types) =>
            Expression.Call(
                IEnumerableOfTypeReferenceAllMethod,
                Expression.Constant(types),
                TargetHasParametersOfTypeBody);

        static Expression<Func<TypeReference, bool>> CreateTargetHasParametersOfTypeBody()
        {
            var typeParameter = Expression.Parameter(typeof(TypeReference), "t"); //t

            var parameterParameter = Expression.Parameter(typeof(ParameterDefinition), "p"); //p

            var parameterParameterType = Expression.Property(
                parameterParameter,
                typeof(ParameterDefinition).GetProperty(nameof(ParameterDefinition.ParameterType))); //p.ParameterType

            var parameterParameterTypeFullName = Expression.Property(
                parameterParameterType,
                typeof(TypeReference).GetProperty(nameof(TypeReference.FullName))); //p.ParameterType.FullName
            
            var typeParameterFullName = Expression.Property(
                typeParameter,
                typeof(TypeReference).GetProperty(nameof(TypeReference.FullName))); //t.FullName
            
            var parameterTypeEqual = Expression.Equal(parameterParameterTypeFullName, typeParameterFullName); //p.ParameterType.FullName == t.FullName

            var parametersAnyLambda = Expression.Lambda<Func<ParameterDefinition, bool>>(parameterTypeEqual, parameterParameter); //p => p.ParameterType.FullName == t.FullName

            var targetParameters = Expression.Property(
                Target,
                typeof(MethodDefinition).GetProperty(nameof(MethodDefinition.Parameters))); //m.Parameters

            var targetParametersAny = Expression.Call(
                typeof(Enumerable)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .First(m => m.Name == nameof(Enumerable.Any) && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(ParameterDefinition)),
                targetParameters,
                parametersAnyLambda); //m.Parameters.Any(p => p.ParameterType.FullName == t.FullName)

            return Expression.Lambda<Func<TypeReference, bool>>(targetParametersAny, typeParameter); //t => m.Parameters.Any(p => p.ParameterType.FullName == t.FullName)
        }

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