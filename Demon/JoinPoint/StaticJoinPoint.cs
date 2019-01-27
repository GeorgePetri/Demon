using System.Reflection;

namespace Demon.JoinPoint
{
    //todo add MethodInfo
    public class StaticJoinPoint
    {
        public StaticJoinPoint(TypeInfo declaringType) => DeclaringType = declaringType;

        public TypeInfo DeclaringType { get; }
    }
}