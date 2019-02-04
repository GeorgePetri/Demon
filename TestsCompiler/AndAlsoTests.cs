using DemonWeaver;
using DemonWeaver.PointcutExpressionCompiler;
using Mono.Cecil;
using TestsCompiler.Helpers;
using Xunit;

namespace TestsCompiler
{
    public class AndAlsoTests
    {
        readonly ModuleDefinition _module = ModuleDefinition.ReadModule("TestDataForCompiler.dll");

        [Theory]
        [InlineData(@"Within(TestDataForCompiler.Controllers.**) Within(**.Get) &&")]
        [InlineData(@"Within(TestDataForCompiler.Controllers.**) Within(**.Post) ! &&")]
        public void Within(string expression)
        {
            //act
            var func = Compiler.Compile(new PointcutExpression(expression, null), null);

            var result = _module.FilterModule(func);

            //assert   
            Assert.Equal(2, result.Count);
            Assert.Equal("Get", result[0].Name);
            Assert.Equal("Get", result[1].Name);
        }

        [Theory]
        [InlineData(@"&& Within(TestDataForCompiler.Controllers.**) Within(**.Get)")]
        [InlineData(@"Within(TestDataForCompiler.Controllers.**) && Within(**.Get)")]
        public void Throws_IfIsFirstOrSecondToken(string expression)
        {
            //arrange
            var compiler = new Compiler(new PointcutExpression(expression, null), null);

            //assert   
            Assert.Throws<WeavingException>(() => compiler.Compile());
        }
    }
}