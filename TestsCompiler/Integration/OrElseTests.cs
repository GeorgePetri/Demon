using DemonWeaver;
using DemonWeaver.ExpressionCompiler;
using DemonWeaver.ExpressionCompiler.Data;
using Mono.Cecil;
using TestsCompiler.Helpers;
using Xunit;

namespace TestsCompiler.Integration
{
    //todo test variadic
    public class OrElseTests
    {
        readonly ModuleDefinition _module = ModuleDefinition.ReadModule("TestDataForCompiler.dll");

        [Fact]
        public void Within()
        {
            //arrange
            const string expression = @"(or (within @TestDataForCompiler.Services.**) (within @**.Post))";

            //act
            var func = Compiler.Compile(new PointcutExpression(expression, null), null);

            var result = _module.FilterModule(func);

            //assert   
            Assert.Equal(4, result.Count);
        }

        [Theory]
        [InlineData(@"(or)")]
        [InlineData(@"(or (within @**.Get))")]
        public void Throws_IfNotAtLeast2Args(string expression)
        {
            //arrange
            var compiler = new Compiler(new PointcutExpression(expression, null), null);

            //assert   
            Assert.Throws<WeavingException>(() => compiler.Compile());
        }
    }
}