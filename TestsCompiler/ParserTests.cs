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
    }
}