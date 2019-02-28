using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            ModuleDefinition demonModule = null;
            try
            {
                demonModule = HackyLoadDemonModule();

                foreach (var path in assemblyPaths)
                    modules.Add(ModuleDefinition.ReadModule(path, new ReaderParameters {ReadWrite = true}));

                var allTypes = modules.SelectMany(m => m.GetTypes()).ToList();

                var advice = AspectModelBuilder.FromTypeDefinitions(allTypes);

                //todo filter to not run on aspects accidentally
                foreach (var type in allTypes)
                    TypeWeaver.Weave(type, advice, demonModule);

                //todo check file locking issues
                foreach (var module in modules)
                    module.Write();
            }
            finally
            {
                foreach (var module in modules)
                    module.Dispose();

                demonModule?.Dispose();
            }
        }

        //todo kinda hacky but works
        static ModuleDefinition HackyLoadDemonModule()
        {
            var currentCodebasePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;

            var lengthOfDemonWeaverDllName = "/DemonWeaver.dll".Length;
            
            var withoutDll = currentCodebasePath.Substring(0, currentCodebasePath.Length - lengthOfDemonWeaverDllName);

            var demonPath = withoutDll + @"/../lib/netstandard2.0/Demon.dll";

            return ModuleDefinition.ReadModule(demonPath);
        }
    }
}