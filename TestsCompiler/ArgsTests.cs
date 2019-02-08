using System.Linq;
using DemonWeaver;
using DemonWeaver.PointcutExpressionCompiler;
using Mono.Cecil;
using Mono.Collections.Generic;
using TestsCompiler.Helpers;
using Xunit;

namespace TestsCompiler
{
    public class ArgsTests
    {
        readonly ModuleDefinition _module = ModuleDefinition.ReadModule("TestDataForCompiler.dll");

        Collection<MethodDefinition> ArgsMethods => _module
            .Types
            .First(t => t.Name == "ArgsMethods")
            .Methods;

        [Theory]
        [InlineData(@"Args()")]
        [InlineData(@"Args(   )")]
        public void Empty(string expression)
        {
            //arrange
            var emptyArgsMethod = ArgsMethods.First(m => m.Name == "Empty");

            //act
            var func = Compiler.Compile(new PointcutExpression(expression, emptyArgsMethod), null);

            var result = _module.FilterModule(func);

            //assert
            Assert.Contains(result, m => m.Name == "Empty" && m.DeclaringType.Name == "ArgsMethods");
        }

        [Theory]
        [InlineData(@"Args(/)")]
        [InlineData(@"Args(&a)")]
        [InlineData(@"Args($)")]
        [InlineData(@"Args(^c)")]
        public void ThrowOnInvalidArgumentNames(string expression)
        {
            //arrange
            var emptyArgsMethod = ArgsMethods.First(m => m.Name == "Empty");

            //act
            var compiler = new Compiler(new PointcutExpression(expression, emptyArgsMethod), null);

            //assert
            Assert.Throws<WeavingException>(() => compiler.Compile());
        }

        [Theory]
        [InlineData(@"Args(i)")]
        [InlineData(@"Args(is)")]
        [InlineData(@"Args(s,i)")]
        public void ThrowOnArgumentNotFoundOnDefiningMethod(string expression)
        {
            //arrange
            var emptyArgsMethod = ArgsMethods.First(m => m.Name == "Empty");

            //act
            var compiler = new Compiler(new PointcutExpression(expression, emptyArgsMethod), null);

            //assert
            Assert.Throws<WeavingException>(() => compiler.Compile());
        }

        [Fact]
        public void OneSingleStar()
        {
            //arrange
            const string expression = @"Args(*)";

            var emptyArgsMethod = ArgsMethods.First(m => m.Name == "Empty");

            //act
            var func = Compiler.Compile(new PointcutExpression(expression, emptyArgsMethod), null);

            var result = _module.FilterModule(func);

            //assert
            Assert.Contains(result, m => m.Name == "OneInt" && m.DeclaringType.Name == "ArgsMethods");
        } 
        
        [Fact]
        public void TwoSingleStars()
        {
            //arrange
            const string expression = @"Args(*,*)";

            var emptyArgsMethod = ArgsMethods.First(m => m.Name == "Empty");

            //act
            var func = Compiler.Compile(new PointcutExpression(expression, emptyArgsMethod), null);

            var result = _module.FilterModule(func);

            //assert
            Assert.Contains(result, m => m.Name == "TwoInt" && m.DeclaringType.Name == "ArgsMethods");
        }        
        
        [Fact]
        public void OneDoubleStar()
        {
            //arrange
            const string expression = @"Args(**)";

            var emptyArgsMethod = ArgsMethods.First(m => m.Name == "Empty");

            //act
            var func = Compiler.Compile(new PointcutExpression(expression, emptyArgsMethod), null);

            var result = _module.FilterModule(func);

            //assert
            Assert.Contains(result, m => m.Name == "OneInt" && m.DeclaringType.Name == "ArgsMethods");
            Assert.Contains(result, m => m.Name == "TwoInt" && m.DeclaringType.Name == "ArgsMethods");
        }    
        
        [Fact]
        public void OneDoubleStarAndOneSingleStar()
        {
            //arrange
            const string expression = @"Args(**, *)";

            var emptyArgsMethod = ArgsMethods.First(m => m.Name == "Empty");

            //act
            var func = Compiler.Compile(new PointcutExpression(expression, emptyArgsMethod), null);

            var result = _module.FilterModule(func);

            //assert
            Assert.Contains(result, m => m.Name == "TwoInt" && m.DeclaringType.Name == "ArgsMethods");
        }  
        
        [Fact]
        public void OneInt()
        {
            //arrange
            const string expression = @"Args(i)";

            var oneIntMethod = ArgsMethods.First(m => m.Name == "OneInt");

            //act
            var func = Compiler.Compile(new PointcutExpression(expression, oneIntMethod), null);

            var result = _module.FilterModule(func);

            //assert
            Assert.Contains(result, m => m.Name == "OneInt" && m.DeclaringType.Name == "ArgsMethods");
        }
    }
}