using Demon.Fody.PointcutExpression;
using Fody;
using Mono.Cecil;
using TestsCompiler.Helpers;
using Xunit;

namespace TestsCompiler
{
    public class NotTests
    {
        private readonly ModuleDefinition _module = ModuleDefinition.ReadModule("TestDataForCompiler.dll");

        [Theory]
        [InlineData(@"Within(TestDataForCompiler.Services.UserService.Get) !")]
        [InlineData(@"Within(TestDataForCompiler.Services.UserService.Get)!")]
        public void Negates_Within(string expression)
        {
            //arrange
            var compiler = new Compiler(expression);

            //act
            var func = compiler.Compile();

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
            var compiler = new Compiler(expression);

            //assert   
            Assert.Throws<WeavingException>(() => compiler.Compile());
        }
    }
}