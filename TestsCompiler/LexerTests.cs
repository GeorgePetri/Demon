using System.Linq;
using DemonWeaver;
using DemonWeaver.ExpressionCompiler;
using DemonWeaver.ExpressionCompiler.Token;
using Xunit;

namespace TestsCompiler
{
    public class LexerTests
    {
        [Theory]
        [InlineData(@"and ( ) within")]
        [InlineData(@" and ( ) within ")]
        [InlineData(@"and  (  )  within")]
        [InlineData(@"
and
(
)
within")]
        void ReturnsTokens_WhenMatchingEverything(string expression)
        {
            //act
            var tokens = Lexer.AnalyseExpression(expression).ToList();

            //assert
            Assert.IsType<AndAlsoToken>(tokens[0]);
            Assert.IsType<LeftParenToken>(tokens[1]);
            Assert.IsType<RightParenToken>(tokens[2]);
            Assert.IsType<WithinToken>(tokens[3]);
        }

        [Fact]
        void ReturnsEmpty_WhenWhitespace()
        {
            //act
            var tokens = Lexer.AnalyseExpression(" ");
            
            //assert
            Assert.Empty(tokens);
        }
        
        [Theory]
        [InlineData(@"/")]
        [InlineData(@"*")]
        [InlineData(@"%")]
        [InlineData(@"1")]
        [InlineData(@"-")]
        [InlineData(@"_")]
        void Throws_WhenNotMatchingEverything(string expression)
        {
            //assert
            Assert.Throws<WeavingException>(() => Lexer.AnalyseExpression(expression).ToList());
        }
    }
}