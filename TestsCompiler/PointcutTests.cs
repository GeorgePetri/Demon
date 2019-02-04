using System.Collections.Generic;
using DemonWeaver.PointcutExpressionCompiler;
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

            var pointcutDictionary = new Dictionary<string, PointcutExpression> {{pointcutKey, new PointcutExpression(pointcutExpression, null)}};
            var context = new PointcutContext(pointcutDictionary);

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
            const string pointcutExpression = @"Within(TestDataForCompiler.Services.UserService.Get)!";
            const string pointcutKey = "UserService";
            const string expression = pointcutKey + @"()";

            var pointcutDictionary = new Dictionary<string, PointcutExpression> {{pointcutKey, new PointcutExpression(pointcutExpression, null)}};
            var context = new PointcutContext(pointcutDictionary);

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
            const string pointcutExpression = @"Within(TestDataForCompiler.Services.UserService.Get)";
            const string pointcutKey = "UserService";
            const string expression = pointcutKey + @"()!";

            var pointcutDictionary = new Dictionary<string, PointcutExpression> {{pointcutKey, new PointcutExpression(pointcutExpression, null)}};
            var context = new PointcutContext(pointcutDictionary);

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
            const string pointcutInnerExpression = @"Within(TestDataForCompiler.Services.UserService.Get)";
            const string pointcutInnerKey = "UserService";
            const string pointcutOuterExpression = pointcutInnerKey + @"()";
            const string pointcutOuterKey = "UserServiceCall";
            const string expression = pointcutOuterKey + @"()";

            var pointcutDictionary = new Dictionary<string, PointcutExpression>
            {
                {pointcutInnerKey, new PointcutExpression(pointcutInnerExpression, null)},
                {pointcutOuterKey, new PointcutExpression(pointcutOuterExpression, null)}
            };
            var context = new PointcutContext(pointcutDictionary);

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
            const string pointcutInner1Expression = @"Within(TestDataForCompiler.Controllers.UserController.*)";
            const string pointcutInner1Key = "UserController";
            const string pointcutInner2Expression = @"Within(**.Post)";
            const string pointcutInner2Key = "Post";
            const string pointcutOuterExpression = pointcutInner1Key + @"()" + pointcutInner2Key + @"() ! &&";
            const string pointcutOuterKey = "UserControllerAndNotPost";
            const string expression = pointcutOuterKey + @"()";

            var pointcutDictionary = new Dictionary<string, PointcutExpression>
            {
                {pointcutInner1Key, new PointcutExpression(pointcutInner1Expression, null)},
                {pointcutInner2Key, new PointcutExpression(pointcutInner2Expression, null)},
                {pointcutOuterKey, new PointcutExpression(pointcutOuterExpression, null)}
            };
            var context = new PointcutContext(pointcutDictionary);

            //act
            var func = Compiler.Compile(new PointcutExpression(expression, null), context);

            var result = _module.FilterModule(func);

            //assert
            Assert.Single(result);
            Assert.Equal("Get", result[0].Name);
        }
    }
}