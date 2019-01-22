using Demon.Fody.PointcutExpression;
using Fody;
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

            var compiler = new Compiler(expression, null);

            //assert   
            Assert.Throws<WeavingException>(() => compiler.Compile());
        }
    }
}