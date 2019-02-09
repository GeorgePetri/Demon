using DemonWeaver.PointcutExpressionCompiler;
using DemonWeaver.PointcutExpressionCompiler.Token;
using Xunit;

namespace Tests
{
    public class TokenValueParserTests
    {
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
            var result = TokenValueParser.Process(new WithinToken(token));

            //assert           
            Assert.Equal(@"^Assembly\.Class\.Method$", result);
        }

        [Fact]
        public void TryProcessWithin_ReturnsRegex_WhenMatching_SingleStar()
        {
            //arrange
            const string token = @"Within(Assembly.Class.*Method)";

            //act
            var result = TokenValueParser.Process(new WithinToken(token));

            //assert           
            Assert.Equal(@"^Assembly\.Class\.[a-zA-Z1-9]+Method$", result);
        }

        [Fact]
        public void TryProcessWithin_ReturnsRegex_WhenMatching_DoubleStar()
        {
            //arrange
            const string token = @"Within( **.Method)";

            //act
            var result = TokenValueParser.Process(new WithinToken(token));

            //assert           
            Assert.Equal(@"^[a-zA-Z1-9.]+\.Method$", result);
        }
    }
}