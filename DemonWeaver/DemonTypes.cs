using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace DemonWeaver
{
    public class DemonTypes
    {
        DemonTypes
        (
            TypeDefinition joinPoint,
            MethodDefinition joinPointConstructor,
            TypeDefinition typeJoinPoint,
            MethodDefinition typeJoinPointConstructor,
            ParametersTypes parameters,
            ReturnTypes returns
        )
        {
            JoinPoint = joinPoint;
            JoinPointConstructor = joinPointConstructor;
            TypeJoinPoint = typeJoinPoint;
            TypeJoinPointConstructor = typeJoinPointConstructor;
            Parameters = parameters;
            Returns = returns;
        }

        public TypeDefinition JoinPoint { get; }
        public MethodDefinition JoinPointConstructor { get; }
        public TypeDefinition TypeJoinPoint { get; }
        public MethodDefinition TypeJoinPointConstructor { get; }
        public ParametersTypes Parameters { get; }
        public ReturnTypes Returns { get; }

        //todo refac, kinda ugly
        public static DemonTypes FromModule(ModuleDefinition module)
        {
            TypeDefinition joinPoint = default;
            TypeDefinition typeJoinPoint = default;

            TypeDefinition parametersGeneric1 = default;

            TypeDefinition returnAny = default;
            TypeDefinition returnGeneric = default;
            TypeDefinition returnVoid = default;

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
                    case FullNames.ParametersFullNames.Generic1:
                        parametersGeneric1 = type;
                        break;
                    case FullNames.ReturnFullNames.Any:
                        returnAny = type;
                        break;
                    case FullNames.ReturnFullNames.Generic:
                        returnGeneric = type;
                        break;
                    case FullNames.ReturnFullNames.Void:
                        returnVoid = type;
                        break;
                }
            }

            return new DemonTypes(
                joinPoint,
                FirstConstructor(joinPoint),
                typeJoinPoint,
                FirstConstructor(typeJoinPoint),
                new ParametersTypes(
                    FirstConstructor(parametersGeneric1)),
                new ReturnTypes(
                    FirstConstructor(returnAny),
                    FirstConstructor(returnGeneric),
                    FirstConstructor(returnVoid)));
        }

        static MethodDefinition FirstConstructor(TypeDefinition type) => type.GetConstructors().First();

        public class ParametersTypes
        {
            public ParametersTypes(MethodDefinition genericConstructor1)
            {
                GenericConstructor1 = genericConstructor1;
            }

            public MethodDefinition GenericConstructor1 { get; }
        }

        public class ReturnTypes
        {
            public ReturnTypes(MethodDefinition anyConstructor, MethodDefinition genericConstructor, MethodDefinition voidConstructor)
            {
                AnyConstructor = anyConstructor;
                GenericConstructor = genericConstructor;
                VoidConstructor = voidConstructor;
            }

            public MethodDefinition AnyConstructor { get; }
            public MethodDefinition GenericConstructor { get; }
            public MethodDefinition VoidConstructor { get; }
        }

        public static class FullNames
        {
            public const string AspectAttribute = "Demon.Aspect.AspectAttribute";
            public const string PointcutAttribute = "Demon.Aspect.PointcutAttribute";
            public const string BeforeAttribute = "Demon.Aspect.BeforeAttribute";
            public const string AroundAttribute = "Demon.Aspect.AroundAttribute";

            public const string JoinPoint = "Demon.JoinPoint.JoinPoint`2";
            public const string TypeJoinPoint = "Demon.JoinPoint.TypeJoinPoint";

            public static class ReturnFullNames
            {
                public const string Any = "Demon.JoinPoint.Return.ReturnAny";
                public const string Generic = "Demon.JoinPoint.Return.Return`1";
                public const string Void = "Demon.JoinPoint.Return.ReturnVoid";
            }

            public static class ParametersFullNames
            {
                public const string Generic1 = "Demon.JoinPoint.Parameters.Parameters`1";
            }
        }
    }
}