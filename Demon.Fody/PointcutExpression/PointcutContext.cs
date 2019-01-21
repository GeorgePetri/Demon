using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Mono.Cecil;

namespace Demon.Fody.PointcutExpression
{
    //todo throw nice error if not found in pointcu defs
    //todo can infinite recursion be detected here?
    public class PointcutContext
    {
        readonly IReadOnlyDictionary<string, string> _pointcutDefinitions;
        readonly ConcurrentDictionary<string, Func<MethodDefinition, bool>> _compiledPointcuts = new ConcurrentDictionary<string, Func<MethodDefinition, bool>>();

        public PointcutContext(IReadOnlyDictionary<string, string> pointcutDefinitions) =>
            _pointcutDefinitions = pointcutDefinitions;

        public Func<MethodDefinition, bool> GetResolved(string pointcut) =>
            _compiledPointcuts.GetOrAdd(pointcut, Factory);

        Func<MethodDefinition, bool> Factory(string pointcut) => 
            new Compiler(_pointcutDefinitions[pointcut], this).Compile();
    }
}