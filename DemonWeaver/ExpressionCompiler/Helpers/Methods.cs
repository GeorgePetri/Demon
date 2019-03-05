using System.Reflection;
using System.Text.RegularExpressions;

namespace DemonWeaver.ExpressionCompiler.Helpers
{
    public static class Methods
    {
        public static MethodInfo RegexIsMatchMethod { get; } = typeof(Regex).GetMethod(nameof(Regex.IsMatch), new[] {typeof(string)});
    }
}