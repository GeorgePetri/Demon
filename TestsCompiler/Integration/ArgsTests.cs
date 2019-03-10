using System.Linq;
using DemonWeaver.ExpressionCompiler;
using DemonWeaver.ExpressionCompiler.Data;
using Mono.Cecil;
using Mono.Collections.Generic;
using TestsCompiler.Helpers;
using Xunit;

namespace TestsCompiler.Integration
{
    public class ArgsTests
    {
        readonly ModuleDefinition _module = ModuleDefinition.ReadModule("TestDataForCompiler.dll");

        Collection<MethodDefinition> ArgsMethods => _module
            .Types
            .First(t => t.Name == "ArgsMethods")
            .Methods;

        [Theory]
        [InlineData(@"(args)")]
        [InlineData(@"(args   )")]
        public void Empty(string expression)
        {
            //arrange
            var emptyArgsMethod = ArgsMethods.First(m => m.Name == "Empty");

            //act
            var func = Compiler.Compile(new PointcutExpression(expression, emptyArgsMethod), null);

            var result = _module.FilterModule(func);

            //assert
            Assert.Contains(result, m => m.Name == "Empty" && m.DeclaringType.Name == "ArgsMethods");
        }
    }
}