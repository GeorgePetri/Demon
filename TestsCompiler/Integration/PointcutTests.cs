using System.Collections.Generic;
using DemonWeaver.ExpressionCompiler;
using DemonWeaver.ExpressionCompiler.Data;
using Mono.Cecil;
using TestsCompiler.Helpers;
using Xunit;

namespace TestsCompiler.Integration
{
    public class PointcutTests
    {
        readonly ModuleDefinition _module = ModuleDefinition.ReadModule("TestDataForCompiler.dll");

        [Fact]
        public void SimpleWithin()
        {
            //arrange
            const string pointcutExpression = @"(within @TestDataForCompiler.Services.UserService.Get)";
            const string pointcutKey = "UserService";
            const string expression = "(" + pointcutKey + ")";

            var definitions = new Dictionary<string, PointcutExpression> {{pointcutKey, new PointcutExpression(pointcutExpression, null)}};
            var context = new Environment(definitions);

            //act
            var func = Compiler.Compile(new PointcutExpression(expression, null), context);

            var result = _module.FilterModule(func);

            //assert
            Assert.Single(result);
            Assert.Equal("Get", result[0].Name);
        }
    }
}