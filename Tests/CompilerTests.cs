using System;
using System.Collections.Generic;
using System.Linq;
using Demon.Fody.PointcutExpression;
using Mono.Cecil;
using Xunit;

namespace Tests
{
    //todo split into class that only tests Within
    //todo impl syntax errors in within
    public class CompilerTests
    {
        private readonly ModuleDefinition _module = ModuleDefinition.ReadModule("TestDataForCompiler.dll");

        [Fact]
        public void Within_IsFalse_ForNotWithinTarget()
        {
            //arrange
            const string expression = @"Within(TestDataForCompiler.NotExisting)";

            var compiler = new Compiler(expression);

            //act
            var func = compiler.Compile();

            var result = FilterModule(func);

            //assert
            Assert.False(result.Any());
        }

        [Fact]
        public void Within_IsTrue_ForWithinSpecificTarget()
        {
            //arrange
            const string expression = @"Within(TestDataForCompiler.Services.UserService)";

            var compiler = new Compiler(expression);

            //act
            var func = compiler.Compile();

            var result = FilterModule(func);

            //assert
            Assert.Single(result); 
            Assert.Equal("Get",result[0].Name); 
        }
        
        [Fact]
        public void Within_IsTrue_ForWithinStarTarget()
        {
            //arrange
            const string expression = @"Within(TestDataForCompiler.Services.*)";

            var compiler = new Compiler(expression);

            //act
            var func = compiler.Compile();

            var result = FilterModule(func);

            //assert
            Assert.Equal(2, result.Count); 
            Assert.Equal("Get",result[0].Name); 
            Assert.Equal("Get",result[1].Name); 
        }

        private List<MethodDefinition> FilterModule(Func<MethodDefinition, bool> func) =>
            _module.Types
                .SelectMany(t => t.Methods)
                .Where(func)
                .Where(m => m.Name != ".ctor") //todo impl filtering of ctors in compiler
                .ToList();
    }
}