using System.Reflection;

namespace Demon.JoinPoint
{
    //todo add MethodInfo
    public class TypeJoinPoint
    {
        public TypeJoinPoint(TypeInfo declaringType) => DeclaringType = declaringType;

        public TypeInfo DeclaringType { get; }
    }
}