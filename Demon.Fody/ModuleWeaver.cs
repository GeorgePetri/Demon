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

        public override bool ShouldCleanReference { get; } = false;
    }
};