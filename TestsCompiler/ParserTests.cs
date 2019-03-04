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
            var tokens = new List<IToken> {new LeftParenToken(), new WithinToken(), new StringToken("**.Get*"), new RightParenToken(), new EofToken()};

            //act
            var result = new Parser(tokens).Parse();

            //assert
            Assert.Equal("**.Get*", ((StringSym) result.Pop()).Value);
            Assert.IsType<WithinSym>(result.Pop());
            Assert.Empty(result);
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
            }; //(not (within #**.Get*))

            //act
            var result = new Parser(tokens).Parse();

            //assert
            Assert.Equal("**.Get*", ((StringSym) result.Pop()).Value);
            Assert.IsType<WithinSym>(result.Pop());
            Assert.IsType<NotSym>(result.Pop());
            Assert.Empty(result);
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
            }; // (or (within #**.Get*) (within #**.Set*))

            //act
            var result = new Parser(tokens).Parse();

            //assert
            Assert.Equal("**.Get*", ((StringSym) result.Pop()).Value);
            Assert.IsType<WithinSym>(result.Pop());
            Assert.Equal("**.Set*", ((StringSym) result.Pop()).Value);
            Assert.IsType<WithinSym>(result.Pop());
            Assert.IsType<OrElseSym>(result.Pop());
            Assert.Empty(result);
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
            }; // (and (within #**.Get*) (within #**.Set*))

            //act
            var result = new Parser(tokens).Parse();

            //assert
            Assert.Equal("**.Get*", ((StringSym) result.Pop()).Value);
            Assert.IsType<WithinSym>(result.Pop());
            Assert.Equal("**.Set*", ((StringSym) result.Pop()).Value);
            Assert.IsType<WithinSym>(result.Pop());
            Assert.IsType<AndAlsoSym>(result.Pop());
            Assert.Empty(result);
        }
    }
}