using System.Linq;
using DemonWeaver.PointcutExpressionCompiler;
using Mono.Cecil;
using TestsCompiler.Helpers;
using Xunit;

namespace TestsCompiler
{
    public class WithinTests
    {
        //todo impl syntax errors in within
        readonly ModuleDefinition _module = ModuleDefinition.ReadModule("TestDataForCompiler.dll");

        [Theory]
        [InlineData(@"Within(TestDataForCompiler.*)")]
        [InlineData(@"Within(TestDataForCompiler.NotExisting)")]
        public void IsFalse_ForNotWithinTarget(string expression)
        {
            //act
            var func = Compiler.Compile(new PointcutExpression(expression, null), null);

            var result = _module.FilterModule(func);

            //assert
            Assert.False(result.Any());
        }

        [Fact]
        public void IsTrue_ForWithinSpecificTarget()
        {
            //arrange
            const string expression = @"Within(TestDataForCompiler.Services.UserService.Get)";

            //act
            var func = Compiler.Compile(new PointcutExpression(expression, null), null);

            var result = _module.FilterModule(func);

            //assert
            Assert.Single(result);
            Assert.Equal("Get", result[0].Name);
        }

        [Theory]
        [InlineData(@"Within(TestDataForCompiler.Services.*.*)")]
        [InlineData(@"Within(TestDataForCompiler.Services.*Service.*)")]
        public void IsTrue_ForWithinStarTarget(string expression)
        {
            //act
            var func = Compiler.Compile(new PointcutExpression(expression, null), null);

            var result = _module.FilterModule(func);

            //assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Get", result[0].Name);
            Assert.Equal("Get", result[1].Name);
        }

        [Theory]
        [InlineData(@"Within(TestDataForCompiler.Services.**)")]
        [InlineData(@"Within(TestDataForCompiler.Services.**.**)")]
        [InlineData(@"Within(**Service.*)")]
        public void IsTrue_ForWithinDoubleStarTarget(string expression)
        {
            //act
            var func = Compiler.Compile(new PointcutExpression(expression, null), null);

            var result = _module.FilterModule(func);

            //assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Get", result[0].Name);
            Assert.Equal("Get", result[1].Name);
        }
    }
}