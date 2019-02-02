using DemonWeaver.PointcutExpressionCompiler;
using Xunit;

namespace Tests
{
    public class RegexFactoryTests
    {
        [Theory]
        [InlineData(@"")]
        [InlineData(@"Executing(* **(**)")]
        [InlineData(@"Within()")]
        [InlineData(@"Within(-)")]
        [InlineData(@"Within(_)")]
        [InlineData(@"Within(=)")]
        public void TryProcessWithin_ReturnsNull_WhenNotMatching(string token)
        {
            //act
            var result = RegexFactory.TryProcessWithin(token);

            //assert
            Assert.Null(result);
        }       
        
        [Theory]
        [InlineData(@"Within(Assembly.Class.Method)")]
        [InlineData(@"Within( Assembly.Class.Method)")]
        [InlineData(@"Within(Assembly.Class.Method )")]
        [InlineData(@"Within(   Assembly.Class.Method)")]
        [InlineData(@"Within(Assembly.Class.Method   )")]
        [InlineData(@"Within(   Assembly.Class.Method   )")]
        public void TryProcessWithin_ReturnsRegex_WhenMatching_NoStars_VariousWhitespaces(string token)
        {
            //act
            var result = RegexFactory.TryProcessWithin(token);

            //assert           
            Assert.Equal(@"Assembly\.Class\.Method$",result.ToString());
        }
        
        [Fact]
        public void TryProcessWithin_ReturnsRegex_WhenMatching_SingleStar()
        {
            //arrange
            const string token = @"Within(Assembly.Class.*Method)";
            
            //act
            var result = RegexFactory.TryProcessWithin(token);

            //assert           
            Assert.Equal(@"Assembly\.Class\.[a-zA-Z1-9]+Method$",result.ToString());
        }
        
        [Fact]
        public void TryProcessWithin_ReturnsRegex_WhenMatching_DoubleStar()
        {
            //arrange
            const string token = @"Within( **.Method)";
            
            //act
            var result = RegexFactory.TryProcessWithin(token);

            //assert           
            Assert.Equal(@"[a-zA-Z1-9.]+\.Method$",result.ToString());
        }
    }
}