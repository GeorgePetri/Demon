using System.IO;
using System.Reflection;
using DemonWeaver;
using Mono.Cecil;

namespace TestsWeaving.Helpers
{
    public class WeavedInMemoryModule
    {
        public Assembly Assembly { get; }

        //todo copy pasted from solution weaver
        public WeavedInMemoryModule()
        {
            using (var module = ModuleDefinition.ReadModule(TestDataFilename))
            {
                var types = module.Types;

                var advice = AspectModelBuilder.FromTypeDefinitions(types);

                foreach (var type in types)
                    TypeWeaver.Weave(type, advice);

                using (var stream = new MemoryStream())
                {
                    module.Write(stream);

                    Assembly = Assembly.Load(stream.ToArray());
                }
            }
        }

        static string TestDataFilename => @"TestDataForWeaving.dll";
    }
}