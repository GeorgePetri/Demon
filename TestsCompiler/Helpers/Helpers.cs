using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace TestsCompiler.Helpers
{
    internal static class Helpers
    {
        //todo test for props
        internal static List<MethodDefinition> FilterModule(this ModuleDefinition moduleDefinition, Func<MethodDefinition, bool> func) =>
            moduleDefinition.Types
                .SelectMany(t => t.Methods)
                .Where(func)
                .Where(m => m.Name != ".ctor") //todo impl filtering of ctors in compiler
                .ToList();
    }
}