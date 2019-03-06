using DemonWeaver;
using DemonWeaver.ExpressionCompiler;
using DemonWeaver.ExpressionCompiler.Data;
using Mono.Cecil;
using TestsCompiler.Helpers;
using Xunit;

namespace TestsCompiler.Integration
{
    public class NotTests
    {
        readonly ModuleDefinition _module = ModuleDefinition.ReadModule("TestDataForCompiler.dll");

        [Fact]
        public void Negates_Within()
        {
            //arrange
            const string expression = @"(not (within @TestDataForCompiler.Services.UserService.Get))";

            //act
            var func = Compiler.Compile(new PointcutExpression(expression, null), null);

            var result = _module.FilterModule(func);

            //assert   
            Assert.DoesNotContain(result, m => m.DeclaringType.Name == "UserService");
        }

        [Fact]
        public void Throws_IfNothingAfter()
        {
            //arrange
            const string expression = @"(not)";

            //arrange
            var compiler = new Compiler(new PointcutExpression(expression, null), null);

            //assert   
            Assert.Throws<WeavingException>(() => compiler.Compile());
        }
    }
}