using System.Collections.Generic;
using DemonWeaver.ExpressionCompiler.Token.Interface;

namespace DemonWeaver.ExpressionCompiler
{
    //handle eof
    public class Parser
    {
        readonly List<IToken> _tokens;
        readonly Stack<IToken> _stack = new Stack<IToken>(); //make another type

        public Parser(List<IToken> tokens)
        {
            _tokens = tokens;
        }

        public void Parse()
        {
        }
    }
}

//( and ( within **.get* ) endpoints )
//endpoints = ( within x.y.controllers.*.* )