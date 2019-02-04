using DemonWeaver;
using DemonWeaver.PointcutExpressionCompiler;
using Mono.Cecil;
using TestsCompiler.Helpers;
using Xunit;

namespace TestsCompiler
{
    public class OrElseTests
    {
        readonly ModuleDefinition _module = ModuleDefinition.ReadModule("TestDataForCompiler.dll");

        [Fact]
        public void Within()
        {
            //arrange
            const string expression = @"Within(TestDataForCompiler.Services.**) Within(**.Post) ||";

            //act
            var func = Compiler.Compile(new PointcutExpression(expression, null), null);

            var result = _module.FilterModule(func);

            //assert   
            Assert.Equal(4, result.Count);
        }

        [Theory]
        [InlineData(@"|| Within(TestDataForCompiler.Controllers.**) Within(**.Get)")]
        [InlineData(@"Within(TestDataForCompiler.Controllers.**) || Within(**.Get)")]
        public void Throws_IfIsFirstOrSecondToken(string expression)
        {
            //arrange
            var compiler = new Compiler(new PointcutExpression(expression, null), null);

            //assert   
            Assert.Throws<WeavingException>(() => compiler.Compile());
        }
    }
}