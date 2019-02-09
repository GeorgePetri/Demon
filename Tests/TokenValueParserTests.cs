using DemonWeaver.PointcutExpressionCompiler;
using DemonWeaver.PointcutExpressionCompiler.Token;
using Xunit;

namespace Tests
{
    public class TokenValueParserTests
    {
        [Theory]
        [MemberData(nameof(WithinData_NoStarsVariousWhiteSpaces))]
        [MemberData(nameof(WithinData_SingleStarVariousWhiteSpaces))]
        [MemberData(nameof(WithinData_DoubleStarVariousWhiteSpaces))]
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
        
        public static TheoryData<string, string> WithinData_DoubleStarVariousWhiteSpaces() => new TheoryData<string, string>
        {
            {@"^Assembly\.Class\.[\w.]+$", @"Within(Assembly.Class.**)"},
            {@"^Assembly\.Class\.[\w.]+$", @"Within( Assembly.Class.** )"},
            {@"^Assembly\.Class\.[\w.]+$", @"Within(  Assembly.Class.**  )"},
            
            {@"^[\w.]+\.Class\.[\w.]+Method$", @"Within(**.Class.**Method)"},
            {@"^[\w.]+\.Class\.[\w.]+Method$", @"Within( **.Class.**Method )"},
            {@"^[\w.]+\.Class\.[\w.]+Method$", @"Within(  **.Class.**Method  )"},
        };
    }
}