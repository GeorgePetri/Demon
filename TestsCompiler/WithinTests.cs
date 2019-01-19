using System;
using System.Collections.Generic;
using System.Linq;
using Demon.Fody.PointcutExpression;
using Mono.Cecil;
using Xunit;

namespace TestsCompiler
{
    public class WithinTests
    {
        //todo impl syntax errors in within
        private readonly ModuleDefinition _module = ModuleDefinition.ReadModule("TestDataForCompiler.dll");

        [Theory]
        [InlineData(@"Within(TestDataForCompiler.*)")]
        [InlineData(@"Within(TestDataForCompiler.NotExisting)")]
        public void IsFalse_ForNotWithinTarget(string expression)
        {
            //arrange
            var compiler = new Compiler(expression);

            //act
            var func = compiler.Compile();

            var result = FilterModule(func);

            //assert
            Assert.False(result.Any());
        }

        [Fact]
        public void IsTrue_ForWithinSpecificTarget()
        {
            //arrange
            const string expression = @"Within(TestDataForCompiler.Services.UserService.Get)";

            var compiler = new Compiler(expression);

            //act
            var func = compiler.Compile();

            var result = FilterModule(func);

            //assert
            Assert.Single(result);
            Assert.Equal("Get", result[0].Name);
        }

        [Theory]
        [InlineData(@"Within(*.*.*.*)")]
        [InlineData(@"Within(TestDataForCompiler.*.*.*)")]
        [InlineData(@"Within(TestDataForCompiler.Services.*.*)")]
        [InlineData(@"Within(TestDataForCompiler.Services.*Service.*)")]
        public void IsTrue_ForWithinStarTarget(string expression)
        {
            //arrange
            var compiler = new Compiler(expression);

            //act
            var func = compiler.Compile();

            var result = FilterModule(func);

            //assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Get", result[0].Name);
            Assert.Equal("Get", result[1].Name);
        }

        [Theory]
        [InlineData(@"Within(TestDataForCompiler**)")]
        [InlineData(@"Within(TestDataForCompiler.**)")]
        [InlineData(@"Within(TestDataForCompiler.Services.**)")]
        [InlineData(@"Within(TestDataForCompiler.Services.**.**)")]
        [InlineData(@"Within(**Service.*)")]
        public void IsTrue_ForWithinDoubleStarTarget(string expression)
        {
            //arrange
            var compiler = new Compiler(expression);

            //act
            var func = compiler.Compile();

            var result = FilterModule(func);

            //assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Get", result[0].Name);
            Assert.Equal("Get", result[1].Name);
        }

        //todo test for props
        private List<MethodDefinition> FilterModule(Func<MethodDefinition, bool> func) =>
            _module.Types
                .SelectMany(t => t.Methods)
                .Where(func)
                .Where(m => m.Name != ".ctor") //todo impl filtering of ctors in compiler
                .ToList();
    }
}