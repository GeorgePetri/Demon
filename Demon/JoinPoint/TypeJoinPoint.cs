using System.Reflection;

namespace Demon.JoinPoint
{
    public class TypeJoinPoint
    {
        public TypeJoinPoint(MethodInfo method) => Method = method;

        public MethodInfo Method { get; }
    }
}