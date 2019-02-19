using System.IO;
using System.Linq;
using System.Reflection;
using DemonWeaver;
using Mono.Cecil;

namespace TestsWeaving.Helpers
{
    public class WeavedInMemoryModule
    {
        public Assembly Assembly { get; }
        public Assembly AssemblyDependency { get; }

        //todo copy pasted from solution weaver
        public WeavedInMemoryModule()
        {
            using (ModuleDefinition module = ModuleDefinition.ReadModule(TestDataFilename),
                moduleDependency = ModuleDefinition.ReadModule(TestDataDependencyFilename))
            {
                var types = module.Types.Concat(moduleDependency.Types).ToList();

                var advice = AspectModelBuilder.FromTypeDefinitions(types);

                foreach (var type in types)
                    TypeWeaver.Weave(type, advice);

                using (MemoryStream stream = new MemoryStream(),
                    dependencyStream = new MemoryStream())
                {
                    module.Write(stream);
                    moduleDependency.Write(dependencyStream);

                    Assembly = Assembly.Load(stream.ToArray());
                    AssemblyDependency = Assembly.Load(dependencyStream.ToArray());
                }
            }
        }

        const string TestDataFilename = "TestDataForWeaving.dll";
        const string TestDataDependencyFilename = "TestDataForWeavingDependency.dll";
    }
}