using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace DemonWeaver
{
    public class DemonTypes
    {
        DemonTypes(TypeDefinition typeJoinPoint, MethodDefinition typeJoinPointConstructor)
        {
            TypeJoinPoint = typeJoinPoint;
            TypeJoinPointConstructor = typeJoinPointConstructor;
        }

        public TypeDefinition TypeJoinPoint { get; }
        public MethodDefinition TypeJoinPointConstructor { get; }

        public static DemonTypes FromModule(ModuleDefinition module)
        {
            TypeDefinition typeJoinPoint = default;
            foreach (var type in module.Types)
            {
                if (type.FullName == "Demon.JoinPoint.TypeJoinPoint")
                {
                    typeJoinPoint = type;
                    break;
                }
            }

            return new DemonTypes(typeJoinPoint, typeJoinPoint.GetConstructors().First());
        }
    }
}