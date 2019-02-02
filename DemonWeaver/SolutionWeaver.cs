using System.Linq;
using Demon.Fody;
using Mono.Cecil;

namespace DemonWeaver
{
    //todo parallelize?
    //todo don't weave if not needeed
    public static class SolutionWeaver
    {
        public static void Weave(string[] assemblyPaths)
        {
            var pathsAndModules =
                (from path in assemblyPaths
                    let module = ModuleDefinition.ReadModule(path, new ReaderParameters {ReadWrite = true})
                    select (name: path, module))
                .ToList();

            var allTypes = pathsAndModules.SelectMany(t => t.module.GetTypes()).ToList();

            var advice = AspectModelBuilder.FromTypeDefinitions(allTypes);

            //todo filter to not run on aspects accidentally
            //todo run in paralel
            foreach (var type in allTypes)
                TypeWeaver.Weave(type, advice);

            foreach (var (path, module) in pathsAndModules)
            {
                //todo remove path if not needed
                //todo check file locking issues
                module.Write();
            }
        }
    }
}