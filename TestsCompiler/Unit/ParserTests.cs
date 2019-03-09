using System.Collections.Generic;
using DemonWeaver.ExpressionCompiler;
using DemonWeaver.ExpressionCompiler.Sym;
using DemonWeaver.ExpressionCompiler.Token;
using DemonWeaver.ExpressionCompiler.Token.Interface;
using Xunit;

namespace TestsCompiler.Unit
{
    //todo test errors
    //todo validate symbol
    public class ParserTests
    {
        [Fact]
        void SimpleWithin()
        {
            //arrange
            var tokens = new List<IToken> {new LeftParenToken(), new WithinToken(), new StringToken("**.Get*"), new RightParenToken(), new EofToken()};

            //act
            var result = new Parser(tokens).Parse();

            //assert
            Assert.Equal("**.Get*", ((StringSym) result[0]).Value);
            Assert.IsType<WithinSym>(result[1]);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        void NotWithin()
        {
            //arrange
            var tokens = new List<IToken>
            {
                new LeftParenToken(),
                new NotToken(),
                new LeftParenToken(),
                new WithinToken(),
                new StringToken("**.Get*"),
                new RightParenToken(),
                new RightParenToken(),
                new EofToken()
            }; //(not (within @**.Get*))

            //act
            var result = new Parser(tokens).Parse();

            //assert
            Assert.Equal("**.Get*", ((StringSym) result[0]).Value);
            Assert.IsType<WithinSym>(result[1]);
            Assert.IsType<NotSym>(result[2]);
            Assert.Equal(3, result.Count);
        }


        //todo copy pasted
        [Fact]
        void Or2Within()
        {
            //arrange
            var tokens = new List<IToken>
            {
                new LeftParenToken(),
                new OrElseToken(),
                new LeftParenToken(),
                new WithinToken(),
                new StringToken("**.Get*"),
                new RightParenToken(),
                new LeftParenToken(),
                new WithinToken(),
                new StringToken("**.Set*"),
                new RightParenToken(),
                new RightParenToken(),
                new EofToken()
            }; // (or (within @**.Get*) (within @**.Set*))

            //act
            var result = new Parser(tokens).Parse();

            //assert
            Assert.Equal("**.Get*", ((StringSym) result[0]).Value);
            Assert.IsType<WithinSym>(result[1]);
            Assert.Equal("**.Set*", ((StringSym) result[2]).Value);
            Assert.IsType<WithinSym>(result[3]);
            Assert.IsType<OrElseSym>(result[4]);
            Assert.Equal(5,result.Count);
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
                new StringToken("**.Get*"),
                new RightParenToken(),
                new LeftParenToken(),
                new WithinToken(),
                new StringToken("**.Set*"),
                new RightParenToken(),
                new RightParenToken(),
                new EofToken()
            }; // (and (within @**.Get*) (within @**.Set*))

            //act
            var result = new Parser(tokens).Parse();

            //assert
            Assert.Equal("**.Get*", ((StringSym) result[0]).Value);
            Assert.IsType<WithinSym>(result[1]);
            Assert.Equal("**.Set*", ((StringSym) result[2]).Value);
            Assert.IsType<WithinSym>(result[3]);
            Assert.IsType<AndAlsoSym>(result[4]);
            Assert.Equal(5,result.Count);
        }

        [Fact]
        void SimpleSymbol()
        {
            //arrange
            var tokens = new List<IToken> {new LeftParenToken(), new SymbolToken("endpoints"), new RightParenToken(), new EofToken()};

            //act
            var result = new Parser(tokens).Parse();

            //assert
            Assert.Equal("endpoints", ((SymbolSym) result[0]).Value);
            Assert.Single(result);
        }
    }
}