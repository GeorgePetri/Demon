using System.Linq;
using DemonWeaver;
using DemonWeaver.PointcutExpressionCompiler;
using DemonWeaver.PointcutExpressionCompiler.Token;
using Xunit;

namespace Tests
{
    public class LexerTests
    {
        [Theory]
        [InlineData("Repositories() !Get() && Within(AssemblyToProcess.Repositories.**) || &&")]
        [InlineData(" Repositories()   !Get() &&  Within(AssemblyToProcess.Repositories.**) || &&  ")]
        public void Analyse_ReturnsTokens_WhenMatchingEverything(string expression)
        {
            //act
            var tokens = Lexer.Analyse(expression).ToList();

            //assert
            Assert.IsType<PointcutToken>(tokens[0]);
            Assert.IsType<NotToken>(tokens[1]);
            Assert.IsType<PointcutToken>(tokens[2]);
            Assert.IsType<AndAlsoToken>(tokens[3]);
            Assert.IsType<WithinToken>(tokens[4]);
            Assert.IsType<OrElseToken>(tokens[5]);
            Assert.IsType<AndAlsoToken>(tokens[6]);
        }

        [Theory]
        [InlineData("Repositories() !Get() && Execution(* **(Int,*)) Within(AssemblyToProcess.Repositories.**) || &&  55")]
        [InlineData("Repositories() !Get() && Execution(* **(Int,*)) A( ) Within(AssemblyToProcess.Repositories.**) || &&")]
        [InlineData(" %% Repositories() !Get() && Execution(* **(Int,*)) Within(AssemblyToProcess.Repositories.**) || &&")]
        public void Analyse_Throws_WhenNotMatchingEverything(string expression)
        {
            //assert
            Assert.Throws<WeavingException>(() => Lexer.Analyse(expression).ToList());
        }
    }
}