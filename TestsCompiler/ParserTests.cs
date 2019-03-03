using System.Collections.Generic;
using DemonWeaver.ExpressionCompiler;
using DemonWeaver.ExpressionCompiler.Sym;
using DemonWeaver.ExpressionCompiler.Token;
using DemonWeaver.ExpressionCompiler.Token.Interface;
using Xunit;

namespace TestsCompiler
{
    //todo test errors
    public class ParserTests
    {
        [Fact]
        void SimpleWithin()
        {
            //arrange
            var tokens = new List<IToken> {new LeftParenToken(), new WithinToken(), new SymbolToken("**.Get*"), new RightParenToken(), new EofToken()};

            //act
            var result = new Parser(tokens).Parse();

            //assert
            Assert.Single(result, s => s is WithinSym within && within.Value == "**.Get*");
        }

        [Fact]
        void And2Within()
        {       
            //arrange
            var tokens = new List<IToken>
            {
                new LeftParenToken(),
                new AndAlsoToken(),
                new LeftParenToken(),
                new WithinToken(),
                new SymbolToken("**.Get*"),
                new RightParenToken(),
                new LeftParenToken(),
                new WithinToken(),
                new SymbolToken("**.Set*"),
                new RightParenToken(),
                new RightParenToken(),
                new EofToken()
            };// (and(within **.Get*) (within "**.Set*"))

            //act
            var result = new Parser(tokens).Parse();

            //assert
            Assert.Equal("**.Get*",((WithinSym)result.Pop()).Value);
            Assert.Equal("**.Set*",((WithinSym)result.Pop()).Value);
            Assert.IsType<AndSym>(result.Pop());
            Assert.Empty(result);
        }
    }
}