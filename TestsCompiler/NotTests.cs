using DemonWeaver;
using DemonWeaver.PointcutExpressionCompiler;
using Mono.Cecil;
using TestsCompiler.Helpers;
using Xunit;

namespace TestsCompiler
{
    public class NotTests
    {
        readonly ModuleDefinition _module = ModuleDefinition.ReadModule("TestDataForCompiler.dll");

        [Theory]
        [InlineData(@"Within(TestDataForCompiler.Services.UserService.Get) !")]
        [InlineData(@"Within(TestDataForCompiler.Services.UserService.Get)!")]
        public void Negates_Within(string expression)
        {
            //act
            var func = Compiler.Compile(expression, null);

            var result = _module.FilterModule(func);

            //assert   
            Assert.DoesNotContain(result, m => m.DeclaringType.Name == "UserService");
        }

        [Theory]
        [InlineData(@"! Within(TestDataForCompiler.Services.UserService.Get) ")]
        [InlineData(@"!Within(TestDataForCompiler.Services.UserService.Get)")]
        public void Throws_IfIsFirstToken(string expression)
        {
            //arrange
            var compiler = new Compiler(expression, null);

            //assert   
            Assert.Throws<WeavingException>(() => compiler.Compile());
        }
    }
}