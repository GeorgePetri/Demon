using System.Collections.Generic;
using System.Linq;
using Fody;
using Mono.Cecil;

namespace Demon.Fody
{
    public class ModuleWeaver : BaseModuleWeaver
    {
        public override void Execute()
        {
            var aspects = GetAspects();
            var advice = aspects.SelectMany(GetBeforeAdvice).ToList();

            var objectType = FindType("System.Object");
            var objectImport = ModuleDefinition.ImportReference(objectType);
            ModuleDefinition.Types.Add(new TypeDefinition("MyNamespace", "MyType", TypeAttributes.Public,
                objectImport));
        }

        public override IEnumerable<string> GetAssembliesForScanning()
        {
            //todo add mscorlib?
            yield break;
        }

        private IEnumerable<TypeDefinition> GetAspects() =>
            ModuleDefinition.Types
                .Where(t => t.CustomAttributes
                    .Any(a => a.AttributeType.FullName == "Demon.Aspect.AspectAttribute"));

        private static IEnumerable<(MethodDefinition method, string pointCutExpression)> GetBeforeAdvice(TypeDefinition aspect) =>
            from method in aspect.Methods
            from attribute in method.CustomAttributes
            where attribute.AttributeType.FullName == "Demon.Aspect.BeforeAttribute"
            select (method, (string) attribute.ConstructorArguments[0].Value);

        public override bool ShouldCleanReference { get; } = false;
    }
};