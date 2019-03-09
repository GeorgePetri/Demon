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
            const string pointcutKey = "user-service";
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

        [Fact]
        public void SimpleNegatedWithin()
        {
            //arrange
            const string pointcutExpression = @"(not (within @TestDataForCompiler.Services.UserService.Get))";
            const string pointcutKey = "user-service";
            const string expression = "(" + pointcutKey + ")";

            var definitions = new Dictionary<string, PointcutExpression> {{pointcutKey, new PointcutExpression(pointcutExpression, null)}};
            var context = new Environment(definitions);

            //act
            var func = Compiler.Compile(new PointcutExpression(expression, null), context);

            var result = _module.FilterModule(func);

            //assert  
            Assert.DoesNotContain(result, m => m.Name == "Get" && m.DeclaringType.Name == "UserService");
        }

        [Fact]
        public void SimpleNegatedPointcutWithin()
        {
            //arrange
            const string pointcutExpression = @"(within @TestDataForCompiler.Services.UserService.Get)";
            const string pointcutKey = "user-service";
            const string expression = "(not (" + pointcutKey + "))";

            var definitions = new Dictionary<string, PointcutExpression> {{pointcutKey, new PointcutExpression(pointcutExpression, null)}};
            var context = new Environment(definitions);

            //act
            var func = Compiler.Compile(new PointcutExpression(expression, null), context);

            var result = _module.FilterModule(func);

            //assert  
            Assert.DoesNotContain(result, m => m.Name == "Get" && m.DeclaringType.Name == "UserService");
        }
        
        [Fact]
        public void NestedWithin()
        {
            //arrange
            const string pointcutInnerExpression = @"(within @TestDataForCompiler.Services.UserService.Get)";
            const string pointcutInnerKey = "user-service";
            const string pointcutOuterExpression = "(" + pointcutInnerKey + ")";
            const string pointcutOuterKey = "user-service-call";
            const string expression = "(" + pointcutOuterKey + ")";

            var definitions = new Dictionary<string, PointcutExpression>
            {
                {pointcutInnerKey, new PointcutExpression(pointcutInnerExpression, null)},
                {pointcutOuterKey, new PointcutExpression(pointcutOuterExpression, null)}
            };
            var context = new Environment(definitions);

            //act
            var func = Compiler.Compile(new PointcutExpression(expression, null), context);

            var result = _module.FilterModule(func);

            //assert
            Assert.Single(result);
            Assert.Equal("Get", result[0].Name);
        }

        [Fact]
        public void NestedMultipleComplexWithin()
        {
            //arrange
            const string pointcutInner1Expression = @"(within @TestDataForCompiler.Controllers.UserController.*)";
            const string pointcutInner1Key = "user-controller";
            const string pointcutInner2Expression = @"(within @**.Post)";
            const string pointcutInner2Key = "post";
            const string pointcutOuterExpression = "(and (" +pointcutInner1Key + ")" + "(not(" + pointcutInner2Key + ")))";
            const string pointcutOuterKey = "user-controller-and-not-post";
            const string expression = "(" + pointcutOuterKey + ")";

            var definitions = new Dictionary<string, PointcutExpression>
            {
                {pointcutInner1Key, new PointcutExpression(pointcutInner1Expression, null)},
                {pointcutInner2Key, new PointcutExpression(pointcutInner2Expression, null)},
                {pointcutOuterKey, new PointcutExpression(pointcutOuterExpression, null)}
            };
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