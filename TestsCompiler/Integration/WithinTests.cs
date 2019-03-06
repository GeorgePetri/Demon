using System.Linq;
using DemonWeaver.ExpressionCompiler;
using DemonWeaver.ExpressionCompiler.Data;
using Mono.Cecil;
using TestsCompiler.Helpers;
using Xunit;

namespace TestsCompiler.Integration
{
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
        
        [Fact]
        public void IsTrue_ForWithinSpecificTarget()
        {
            //arrange
            const string expression = @"(within @TestDataForCompiler.Services.UserService.Get)";

            //act
            var func = Compiler.Compile(new PointcutExpression(expression, null), null);

            var result = _module.FilterModule(func);

            //assert
            Assert.Single(result);
            Assert.Equal("Get", result[0].Name);
        }

        [Theory]
        [InlineData(@"(within @TestDataForCompiler.Services.*.*)")]
        [InlineData(@"(within @TestDataForCompiler.Services.*Service.*)")]
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
        [InlineData(@"(within @TestDataForCompiler.Services.**)")]
        [InlineData(@"(within @TestDataForCompiler.Services.**.**)")]
        [InlineData(@"(within @**Service.*)")]
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