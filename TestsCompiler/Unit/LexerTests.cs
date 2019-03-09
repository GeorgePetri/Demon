using System.Linq;
using DemonWeaver;
using DemonWeaver.ExpressionCompiler;
using DemonWeaver.ExpressionCompiler.Token;
using Xunit;

namespace TestsCompiler.Unit
{
    public class LexerTests
    {
        [Theory]
        [InlineData(@"and args ( not or ) @string whatever within")]
        [InlineData(@" and args ( not or ) @string whatever within ")]
        [InlineData(@"and  args  (  not  or  )  @string  whatever  within")]
        [InlineData(@"
and
args
(
not
or
)
@string
whatever
within")]
        void ReturnsTokens_WhenMatchingEverything(string expression)
        {
            //act
            var tokens = Lexer.AnalyseExpression(expression).ToList();

            //assert
            Assert.IsType<AndAlsoToken>(tokens[0]);
            Assert.IsType<ArgsToken>(tokens[1]);
            Assert.IsType<LeftParenToken>(tokens[2]);
            Assert.IsType<NotToken>(tokens[3]);
            Assert.IsType<OrElseToken>(tokens[4]);
            Assert.IsType<RightParenToken>(tokens[5]);
            Assert.IsType<StringToken>(tokens[6]);
            Assert.IsType<SymbolToken>(tokens[7]);
            Assert.IsType<WithinToken>(tokens[8]);
            Assert.IsType<EofToken>(tokens[9]);
        }

        [Theory]
        [InlineData("or(and)or")]
        [InlineData("or ( and ) or")]
        void AnalysesParensWithoutWhitespace(string expression)
        {
            //act
            var tokens = Lexer.AnalyseExpression(expression).ToList();

            //assert
            Assert.IsType<OrElseToken>(tokens[0]);
            Assert.IsType<LeftParenToken>(tokens[1]);
            Assert.IsType<AndAlsoToken>(tokens[2]);
            Assert.IsType<RightParenToken>(tokens[3]);
            Assert.IsType<OrElseToken>(tokens[4]);
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