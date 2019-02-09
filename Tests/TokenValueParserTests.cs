using DemonWeaver.PointcutExpressionCompiler;
using DemonWeaver.PointcutExpressionCompiler.Token;
using Xunit;

namespace Tests
{
    public class TokenValueParserTests
    {
        [Fact]
        public void TryProcessWithin_ReturnsRegex_WhenMatching_DoubleStar()
        {
            //arrange
            const string token = @"Within( **.Method)";

            //act
            var result = TokenValueParser.Process(new WithinToken(token));

            //assert           
            Assert.Equal(@"^[\w.]+\.Method$", result);
        }

        [Theory]
        [MemberData(nameof(WithinData_NoStarsVariousWhiteSpaces))]
        [MemberData(nameof(WithinData_SingleStarVariousWhiteSpaces))]
        public void Within(string expectedResult, string token)
        {
            //act
            var result = TokenValueParser.Process(new WithinToken(token));

            //assert           
            Assert.Equal(expectedResult, result);
        }

        public static TheoryData<string, string> WithinData_NoStarsVariousWhiteSpaces() => new TheoryData<string, string>
        {
            {@"^Assembly\.Class\.Method$", @"Within(Assembly.Class.Method)"},
            {@"^Assembly\.Class\.Method$", @"Within( Assembly.Class.Method)"},
            {@"^Assembly\.Class\.Method$", @"Within(Assembly.Class.Method )"},
            {@"^Assembly\.Class\.Method$", @"Within(   Assembly.Class.Method)"},
            {@"^Assembly\.Class\.Method$", @"Within(Assembly.Class.Method   )"},
            {@"^Assembly\.Class\.Method$", @"Within(   Assembly.Class.Method   )"},
        };

        public static TheoryData<string, string> WithinData_SingleStarVariousWhiteSpaces() => new TheoryData<string, string>
        {
            {@"^Assembly\.Class\.[\w]+Method$", @"Within(Assembly.Class.*Method)"},
            {@"^Assembly\.Class\.[\w]+Method$", @"Within( Assembly.Class.*Method )"},
            {@"^Assembly\.Class\.[\w]+Method$", @"Within(  Assembly.Class.*Method  )"},
            
            {@"^Assembly\.Class[\w]+\.[\w]+Method$", @"Within(Assembly.Class*.*Method)"},
            {@"^Assembly\.Class[\w]+\.[\w]+Method$", @"Within( Assembly.Class*.*Method )"},
            {@"^Assembly\.Class[\w]+\.[\w]+Method$", @"Within(  Assembly.Class*.*Method  )"},
        };
    }
}