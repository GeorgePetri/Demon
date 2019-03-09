using System.Linq;
using DemonWeaver;
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
            var env = CreateEnvironment(("user-service", @"(within @TestDataForCompiler.Services.UserService.Get)"));

            //act
            var func = Compiler.Compile(new PointcutExpression("(user-service)", null), env);

            var result = _module.FilterModule(func);

            //assert
            Assert.Single(result);
            Assert.Equal("Get", result[0].Name);
        }

        [Fact]
        public void SimpleNegatedWithin()
        {
            //arrange
            var env = CreateEnvironment(("user-service", @"(not (within @TestDataForCompiler.Services.UserService.Get))"));

            //act
            var func = Compiler.Compile(new PointcutExpression("(user-service)", null), env);

            var result = _module.FilterModule(func);

            //assert  
            Assert.DoesNotContain(result, m => m.Name == "Get" && m.DeclaringType.Name == "UserService");
        }

        [Fact]
        public void SimpleNegatedPointcutWithin()
        {
            var env = CreateEnvironment(("user-service", @"(within @TestDataForCompiler.Services.UserService.Get)"));

            //act
            var func = Compiler.Compile(new PointcutExpression("(not (user-service))", null), env);

            var result = _module.FilterModule(func);

            //assert  
            Assert.DoesNotContain(result, m => m.Name == "Get" && m.DeclaringType.Name == "UserService");
        }

        [Fact]
        public void NestedWithin()
        {
            //arrange
            var env = CreateEnvironment(
                ("user-service", @"(within @TestDataForCompiler.Services.UserService.Get)"),
                ("user-service-call", "(user-service)"));

            //act
            var func = Compiler.Compile(new PointcutExpression("(user-service-call)", null), env);

            var result = _module.FilterModule(func);

            //assert
            Assert.Single(result);
            Assert.Equal("Get", result[0].Name);
        }

        [Fact]
        public void NestedMultipleComplexWithin()
        {
            //arrange
            var env = CreateEnvironment(
                ("user-controller", @"(within @TestDataForCompiler.Controllers.UserController.*)"),
                ("post", @"(within @**.Post)"),
                ("user-controller-and-not-post", "(and (user-controller) (not (post)))"));

            //act
            var func = Compiler.Compile(new PointcutExpression("(user-controller-and-not-post)", null), env);

            var result = _module.FilterModule(func);

            //assert
            Assert.Single(result);
            Assert.Equal("Get", result[0].Name);
        }

        static Environment CreateEnvironment(params (string key, string expression)[] expressions) =>
            expressions.ToDictionary(t => t.key, t => new PointcutExpression(t.expression, null))
                .Let(d => new Environment(d));
    }
}