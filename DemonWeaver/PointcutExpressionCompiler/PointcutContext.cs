using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Mono.Cecil;

namespace DemonWeaver.PointcutExpressionCompiler
{
    //todo throw nice error if not found in pointcu defs
    //todo can infinite recursion be detected here?
    //todo unit test caching
    public class PointcutContext
    {
        readonly IReadOnlyDictionary<string, PointcutExpression> _pointcutDefinitions;
        readonly ConcurrentDictionary<string, Func<MethodDefinition, bool>> _compiledPointcuts = new ConcurrentDictionary<string, Func<MethodDefinition, bool>>();

        public PointcutContext(IReadOnlyDictionary<string, PointcutExpression> pointcutDefinitions) =>
            _pointcutDefinitions = pointcutDefinitions;

        public Func<MethodDefinition, bool> GetResolved(string pointcut) =>
            _compiledPointcuts.GetOrAdd(pointcut, Factory);

        Func<MethodDefinition, bool> Factory(string pointcut) =>
            Compiler.Compile(_pointcutDefinitions[pointcut], this);
    }
}