using System.Collections.Generic;
using Fody;

namespace Demon.Fody
{
    public class ModuleWeaver : BaseModuleWeaver
    {
        //todo optimize ils, both manually and with cecil function
        public override void Execute()
        {
            var advice = AspectModelBuilder.FromTypeDefinitions(ModuleDefinition.Types);

            //todo filter to not run on aspects accidentally
            //todo run in paralel
            foreach (var type in ModuleDefinition.Types)
                TypeWeaver.Weave(type, advice);
        }

        public override IEnumerable<string> GetAssembliesForScanning()
        {
            //todo add mscorlib?
            yield break;
        }

        public override bool ShouldCleanReference { get; } = false;
    }
}