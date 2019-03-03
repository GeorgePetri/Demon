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
        [InlineData(@"and ( ) whatever within")]
        [InlineData(@" and ( ) whatever within ")]
        [InlineData(@"and  (  )  whatever  within")]
        [InlineData(@"
and
(
)
whatever
within")]
        void ReturnsTokens_WhenMatchingEverything(string expression)
        {
            //act
            var tokens = Lexer.AnalyseExpression(expression).ToList();

            //assert
            Assert.IsType<AndAlsoToken>(tokens[0]);
            Assert.IsType<LeftParenToken>(tokens[1]);
            Assert.IsType<RightParenToken>(tokens[2]);
            Assert.IsType<SymbolToken>(tokens[3]);
            Assert.IsType<WithinToken>(tokens[4]);
            Assert.IsType<EofToken>(tokens[5]);
        }

        [Theory]
        [InlineData("(and)")]
        [InlineData("( and )")]
        void AnalysesParensWithoutWhitespace(string expression)
        {
            //act
            var tokens = Lexer.AnalyseExpression(expression).ToList();

            //assert
            Assert.IsType<LeftParenToken>(tokens[0]);
            Assert.IsType<AndAlsoToken>(tokens[1]);
            Assert.IsType<RightParenToken>(tokens[2]);
        }

        [Fact]
        void ReturnsEof_WhenWhitespace()
        {
            //act
            var tokens = Lexer.AnalyseExpression(" ");

            //assert
            Assert.Single(tokens, t => t is EofToken);
        }

        [Theory]
        [InlineData(@"/")]
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