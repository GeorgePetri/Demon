using System;
using System.Collections.Generic;
using Mono.Cecil;

namespace DemonWeaver.PointcutExpressionCompiler.Data
{
    public class Result
    {
        public Result(Func<MethodDefinition, bool> filter, HashSet<TypeReference> optionalBindings) => 
            (Filter, OptionalBindings) = (filter, optionalBindings);

        public Func<MethodDefinition, bool> Filter { get; }

        public HashSet<TypeReference> OptionalBindings { get; }
    }
}