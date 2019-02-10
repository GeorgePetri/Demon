using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace DemonWeaver
{
    //todo parallelize?
    //todo don't weave if not needed
    public static class SolutionWeaver
    {
        public static void Weave(string[] assemblyPaths)
        {
            var modules = new List<ModuleDefinition>();

            try
            {
                foreach (var path in assemblyPaths)
                    modules.Add(ModuleDefinition.ReadModule(path, new ReaderParameters {ReadWrite = true}));

                var allTypes = modules.SelectMany(m => m.GetTypes()).ToList();

                var advice = AspectModelBuilder.FromTypeDefinitions(allTypes);

                //todo filter to not run on aspects accidentally
                foreach (var type in allTypes)
                    TypeWeaver.Weave(type, advice);

                //todo check file locking issues
                foreach (var module in modules)
                    module.Write();
            }
            finally
            {
                foreach (var module in modules)
                    module.Dispose();
            }
        }
    }
}