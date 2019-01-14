using System.Collections.Generic;
using Fody;

namespace Demon.Fody
{
    public class ModuleWeaver : BaseModuleWeaver
    {
        //todo optimize ils, both manually and with cecil function
        public override void Execute()
        {
            var aspects = AspectDataBuilder.FromTypeDefinitions(ModuleDefinition.Types);

            //todo filter to not run on aspects accidentally
            foreach (var type in ModuleDefinition.Types)
                TypeWeaver.Weave(type, aspects);
        }

        public override IEnumerable<string> GetAssembliesForScanning()
        {
            //todo add mscorlib?
            yield break;
        }

        public override bool ShouldCleanReference { get; } = false;

    }
}