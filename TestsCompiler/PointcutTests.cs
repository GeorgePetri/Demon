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

            //act
            var func = Compiler.Compile(expression, context);

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

            //act
            var func = Compiler.Compile(expression, context);

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

            var pointcutDictionary = new Dictionary<string, string> {{pointcutKey, pointcutExpression}};
            var context = new PointcutContext(pointcutDictionary);

            //act
            var func = Compiler.Compile(expression, context);

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

            var pointcutDictionary = new Dictionary<string, string> {{pointcutInnerKey, pointcutInnerExpression}, {pointcutOuterKey, pointcutOuterExpression}};
            var context = new PointcutContext(pointcutDictionary);

            //act
            var func = Compiler.Compile(expression, context);

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

            var pointcutDictionary = new Dictionary<string, string>
            {
                {pointcutInner1Key, pointcutInner1Expression},
                {pointcutInner2Key, pointcutInner2Expression},
                {pointcutOuterKey, pointcutOuterExpression}
            };
            var context = new PointcutContext(pointcutDictionary);

            //act
            var func = Compiler.Compile(expression, context);

            var result = _module.FilterModule(func);

            //assert
            Assert.Single(result);
            Assert.Equal("Get", result[0].Name);
        }
    }
}