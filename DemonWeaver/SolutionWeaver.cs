using System.Collections.Generic;
using System.Linq;
using Demon.Fody;
using Mono.Cecil;

namespace DemonWeaver
{
    //todo parallelize?
    public class SolutionWeaver
    {
        public static void Weave(List<ModuleDefinition> modules)
        {
            var allTypes = modules.SelectMany(m => m.GetTypes()).ToList();

            var advice = AspectModelBuilder.FromTypeDefinitions(allTypes);

            //todo filter to not run on aspects accidentally
            //todo run in paralel
            foreach (var type in allTypes)
                TypeWeaver.Weave(type, advice);

            
            foreach (var module in modules)
                module.Write();
        }
    }
}