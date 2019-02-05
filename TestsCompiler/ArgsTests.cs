using System.Linq;
using DemonWeaver.PointcutExpressionCompiler;
using Mono.Cecil;
using TestsCompiler.Helpers;
using Xunit;

namespace TestsCompiler
{
    public class ArgsTests
    {
        readonly ModuleDefinition _module = ModuleDefinition.ReadModule("TestDataForCompiler.dll");

        [Theory]
        [InlineData(@"Args()")]
        [InlineData(@"Args(   )")]
        public void Empty(string expression)
        {
            //arrange
            var emptyArgsMethod = _module
                .Types
                .First(t => t.Name == "ArgsMethods")
                .Methods
                .First(m => m.Name == "Empty");

            //act
            var func = Compiler.Compile(new PointcutExpression(expression, emptyArgsMethod), null);

            var result = _module.FilterModule(func);

            //assert
            Assert.Contains(result, m => m.Name == "Empty" && m.DeclaringType.Name == "ArgsMethods");
        }
    }
}