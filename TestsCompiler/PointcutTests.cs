using System.Collections.Generic;
using Demon.Fody.PointcutExpression;
using Mono.Cecil;
using TestsCompiler.Helpers;
using Xunit;

namespace TestsCompiler
{
    public class PointcutTests
    {
        readonly ModuleDefinition _module = ModuleDefinition.ReadModule("TestDataForCompiler.dll");

        [Fact]
        public void SimpleWithin()
        {
            //arrange
            const string pointcutExpression = @"Within(TestDataForCompiler.Services.UserService.Get)";
            const string pointcutKey = "UserService";
            const string expression = pointcutKey + @"()";

            var pointcutDictionary = new Dictionary<string, string> {{pointcutKey, pointcutExpression}};
            var context = new PointcutContext(pointcutDictionary);

            var compiler = new Compiler(expression, context);

            //act
            var func = compiler.Compile();

            var result = _module.FilterModule(func);

            //assert
            Assert.Single(result);
            Assert.Equal("Get", result[0].Name);
        }

        [Fact]
        public void SimpleNegatedWithin()
        {
            //arrange
            const string pointcutExpression = @"Within(TestDataForCompiler.Services.UserService.Get)!";
            const string pointcutKey = "UserService";
            const string expression = pointcutKey + @"()";

            var pointcutDictionary = new Dictionary<string, string> {{pointcutKey, pointcutExpression}};
            var context = new PointcutContext(pointcutDictionary);

            var compiler = new Compiler(expression, context);

            //act
            var func = compiler.Compile();

            var result = _module.FilterModule(func);

            //assert  
            Assert.DoesNotContain(result, m => m.Name == "Get" && m.DeclaringType.Name == "UserService");
        }
    }
}