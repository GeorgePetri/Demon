using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Mono.Cecil;
using Mono.Collections.Generic;

// ReSharper disable PossibleNullReferenceException
// ReSharper disable InconsistentNaming
namespace DemonWeaver.ExpressionCompiler.Helpers
{
    public static class Methods
    {
        public static MethodInfo String_Format { get; } = typeof(string).GetMethod(nameof(string.Format), new[] {typeof(string), typeof(object), typeof(object)});
        
        public static MethodInfo Regex_IsMatch { get; } = typeof(Regex).GetMethod(nameof(Regex.IsMatch), new[] {typeof(string)});

        public static MethodInfo IEnumerableOfTypeReference_All { get; } = typeof(Enumerable)
            .GetMethod(nameof(Enumerable.All))
            .MakeGenericMethod(typeof(TypeReference));

        public static MethodInfo IEnumerableOfParameterDefinition_Any { get; } = typeof(Enumerable)
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .First(m => m.Name == nameof(Enumerable.Any) && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(ParameterDefinition));

        public static PropertyInfo CollectionOfParameterDefinition_Count { get; } = typeof(Collection<ParameterDefinition>)
            .GetProperty(nameof(Collection<ParameterDefinition>.Count));

        public static PropertyInfo MethodDefinition_HasParameters { get; } = typeof(MethodDefinition).GetProperty(nameof(MethodDefinition.HasParameters));
        
        public static PropertyInfo MethodDefinition_Parameters { get; } = typeof(MethodDefinition).GetProperty(nameof(MethodDefinition.Parameters));
        
        public static PropertyInfo MethodDefinition_Name { get; } = typeof(MethodDefinition).GetProperty(nameof(MethodDefinition.Name));
        
        public static PropertyInfo MethodDefinition_DeclaringType { get; } = typeof(MethodDefinition)
            .GetProperty(nameof(MethodDefinition.DeclaringType), typeof(TypeDefinition));
        
        public static PropertyInfo ParameterDefinition_ParameterType { get; } = typeof(ParameterDefinition).GetProperty(nameof(ParameterDefinition.ParameterType));
        
        public static PropertyInfo TypeReference_FullName { get; } = typeof(TypeReference).GetProperty(nameof(TypeReference.FullName));
        
        public static PropertyInfo TypeDefinition_FullName { get; } = typeof(TypeDefinition).GetProperty(nameof(TypeDefinition.FullName));
    }
}