using System.Linq;
using DemonWeaver.ExpressionCompiler;
using DemonWeaver.ExpressionCompiler.Data;
using Mono.Cecil;
using TestsCompiler.Helpers;
using Xunit;

namespace TestsCompiler.Integration
{
    //todo copy test from within
    public class WithinTests
    {
        readonly ModuleDefinition _module = ModuleDefinition.ReadModule("TestDataForCompiler.dll");

        [Theory]
        [InlineData(@"(within @TestDataForCompiler.*)")]
        [InlineData(@"(within @TestDataForCompiler.NotExisting)")]
        public void IsFalse_ForNotWithinTarget(string expression)
        {
            //act
            var func = Compiler.Compile(new PointcutExpression(expression, null), null);

            var result = _module.FilterModule(func);

            //assert
            Assert.False(result.Any());
        }
    }
}