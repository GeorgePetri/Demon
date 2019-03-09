using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Mono.Cecil;

namespace DemonWeaver.ExpressionCompiler.Data
{
    //todo throw nice error if not found in pointcu defs
    //todo can infinite recursion be detected here?
    //todo unit test caching
    public class Environment
    {
        readonly IReadOnlyDictionary<string, PointcutExpression> _definitions;
        readonly ConcurrentDictionary<string, Func<MethodDefinition, bool>> _compiled = new ConcurrentDictionary<string, Func<MethodDefinition, bool>>();

        public Environment(IReadOnlyDictionary<string, PointcutExpression> definitions) => _definitions = definitions;

        public Func<MethodDefinition, bool> Resolve(string key) =>
            _compiled.GetOrAdd(key, Factory);

        Func<MethodDefinition, bool> Factory(string key) =>
            Compiler.Compile(_definitions[key], this);
    }
}