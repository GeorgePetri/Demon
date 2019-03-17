using System;
using System.Reflection;
using System.Text.RegularExpressions;
using DemonWeaver;
using DemonWeaver.ExpressionCompiler;
using DemonWeaver.Extensions;
using Xunit;

namespace TestsCompiler.Unit
{
    public class RegexTests
    {
        static readonly Regex SymbolRegex = (Regex) typeof(Lexer).GetField("SymbolRegex", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
        static readonly Regex StringsRegex = (Regex) typeof(Lexer).GetField("StringRegex", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

        static readonly Func<string, string> ToKebabCase = (Func<string, string>) typeof(AspectModelBuilder)
            .GetMethod("ToKebabCase", BindingFlags.NonPublic | BindingFlags.Static)
            .Let(m => Delegate.CreateDelegate(typeof(Func<string, string>), null, m));

        [Theory]
        [InlineData("b")]
        [InlineData("B")]
        [InlineData("*")]
        [InlineData("b-")]
        [InlineData("b.")]
        [InlineData("b1")]
        [InlineData("qwertyuiop-asdfghjkl-ZXCVBNM*123456789")]
        void Valid_Symbols(string value) => 
            Assert.Matches(SymbolRegex, value);  
        
        [Theory]
        [InlineData("-")]
        [InlineData("1")]
        [InlineData(".")]
        [InlineData("111**.")]
        [InlineData("b`")]
        [InlineData("b@")]
        [InlineData("b#")]
        [InlineData("b$")]
        [InlineData("b%")]
        [InlineData("b^")]
        [InlineData("b&")]
        void Invalid_Symbols(string value) => 
            Assert.DoesNotMatch(SymbolRegex, value);
    }
}