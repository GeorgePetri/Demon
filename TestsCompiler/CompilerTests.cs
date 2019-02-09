using DemonWeaver;
using DemonWeaver.PointcutExpressionCompiler;
using DemonWeaver.PointcutExpressionCompiler.Data;
using Xunit;

namespace TestsCompiler
{
    public class CompilerTests
    {
        [Fact]
        public void MoreThanOneItemOnTheStack()
        {
            //arrange
            const string expression = @"Within(TestDataForCompiler.Services.**) Within(**.Post)";

            var compiler = new Compiler(new PointcutExpression(expression, null), null);

            //assert   
            Assert.Throws<WeavingException>(() => compiler.Compile());
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData(@"")]
        [InlineData(@" ")]
        [InlineData(@"    ")]
        public void EmptyExpression(string expression)
        {
            //arrange
            var compiler = new Compiler(new PointcutExpression(expression, null), null);

            //assert   
            Assert.Throws<WeavingException>(() => compiler.Compile());
        }
    }
}