using System;
using System.Collections.Generic;
using System.Linq;
using Demon.Fody.PointcutExpression;
using Mono.Cecil;
using Xunit;

namespace TestsCompiler
{
    //todo test throws
    public class NotTests
    {
        private readonly ModuleDefinition _module = ModuleDefinition.ReadModule("TestDataForCompiler.dll");

        [Fact]
        public void Negates_Within()
        {
            //arrange
            const string expression = @"Within(TestDataForCompiler.Services.UserService.Get) !";

            var compiler = new Compiler(expression);

            //act
            var func = compiler.Compile();

            var result = FilterModule(func);

            //assert   
            Assert.DoesNotContain(result, m => m.DeclaringType.Name == "UserService");
        }

        //todo test for props
        //todo don't copy paste this
        private List<MethodDefinition> FilterModule(Func<MethodDefinition, bool> func) =>
            _module.Types
                .SelectMany(t => t.Methods)
                .Where(func)
                .Where(m => m.Name != ".ctor") //todo impl filtering of ctors in compiler
                .ToList();
    }
}