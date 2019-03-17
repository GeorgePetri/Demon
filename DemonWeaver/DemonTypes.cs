using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace DemonWeaver
{
    public class DemonTypes
    {
        DemonTypes(MethodDefinition joinPointConstructor, TypeDefinition typeJoinPoint, MethodDefinition typeJoinPointConstructor)
        {
            JoinPointConstructor = joinPointConstructor;
            TypeJoinPoint = typeJoinPoint;
            TypeJoinPointConstructor = typeJoinPointConstructor;
        }

        public MethodDefinition JoinPointConstructor { get; }
        public TypeDefinition TypeJoinPoint { get; }
        public MethodDefinition TypeJoinPointConstructor { get; }

        public static DemonTypes FromModule(ModuleDefinition module)
        {
            TypeDefinition joinPoint = default;
            TypeDefinition typeJoinPoint = default;
            foreach (var type in module.Types)
            {
                switch (type.FullName)
                {
                    case FullNames.TypeJoinPoint:
                        typeJoinPoint = type;
                        break;
                    case FullNames.JoinPoint:
                        joinPoint = type;
                        break;
                }
            }

            return new DemonTypes(joinPoint.GetConstructors().First(), typeJoinPoint, typeJoinPoint.GetConstructors().First());
        }

        public static class FullNames
        {
            public const string AspectAttribute = "Demon.Aspect.AspectAttribute";
            public const string PointcutAttribute = "Demon.Aspect.PointcutAttribute";
            public const string BeforeAttribute = "Demon.Aspect.BeforeAttribute";
            public const string AroundAttribute = "Demon.Aspect.AroundAttribute";

            public const string JoinPoint = "Demon.JoinPoint.JoinPoint`2";
            public const string TypeJoinPoint = "Demon.JoinPoint.TypeJoinPoint";
        }
    }
}