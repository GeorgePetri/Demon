using System.Collections.Generic;
using Fody;
using Mono.Cecil;

namespace Demon.Fody
{
    public class ModuleWeaver : BaseModuleWeaver
    {
        public override void Execute()
        {
            var objectType = FindType("System.Object");
            var objectImport = ModuleDefinition.ImportReference(objectType);
            ModuleDefinition.Types.Add(new TypeDefinition("MyNamespace", "MyType", TypeAttributes.Public,
                objectImport));
        }

        public override IEnumerable<string> GetAssembliesForScanning()
        {
            yield break;
        }
    }
}