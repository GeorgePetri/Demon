using System.Linq;
using DemonWeaver;
using DemonWeaver.ExpressionCompiler;
using DemonWeaver.ExpressionCompiler.Data;
using Mono.Cecil;
using Mono.Collections.Generic;
using TestsCompiler.Helpers;
using Xunit;

namespace TestsCompiler.Integration
{
    public class ArgsTests
    {
        readonly ModuleDefinition _module = ModuleDefinition.ReadModule("TestDataForCompiler.dll");

        Collection<MethodDefinition> ArgsMethods => _module
            .Types
            .First(t => t.Name == "ArgsMethods")
            .Methods;

        [Theory]
        [InlineData(@"(args)")]
        [InlineData(@"(args   )")]
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
        [InlineData(@"(args @i)")]
        [InlineData(@"(args @is)")]
        [InlineData(@"(args @s @i)")]
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
            const string expression = @"(args @*)";

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
            const string expression = @"(args @* @*)";

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
            const string expression = @"(args @**)";

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
            const string expression = @"(args @** @*)";

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
            const string expression = @"(args @i)";

            var oneIntMethod = ArgsMethods.First(m => m.Name == "OneInt");

            //act
            var func = Compiler.Compile(new PointcutExpression(expression, oneIntMethod), null);

            var result = _module.FilterModule(func);

            //assert
            Assert.Contains(result, m => m.Name == "OneInt" && m.DeclaringType.Name == "ArgsMethods");
        }

        [Fact]
        public void OneString()
        {
            //arrange
            const string expression = @"(args @s)";

            var oneIntMethod = ArgsMethods.First(m => m.Name == "OneString");

            //act
            var func = Compiler.Compile(new PointcutExpression(expression, oneIntMethod), null);

            var result = _module.FilterModule(func);

            //assert
            Assert.Contains(result, m => m.Name == "OneString" && m.DeclaringType.Name == "ArgsMethods");
        }

        [Fact]
        public void OneIntOneGuid()
        {
            //arrange
            const string expression = @"(args @i @g)";

            var oneIntMethod = ArgsMethods.First(m => m.Name == "OneIntOneGuid");

            //act
            var func = Compiler.Compile(new PointcutExpression(expression, oneIntMethod), null);

            var result = _module.FilterModule(func);

            //assert
            Assert.Contains(result, m => m.Name == "OneIntOneGuid" && m.DeclaringType.Name == "ArgsMethods");
        }

        [Theory]
        [InlineData(@"(args @i @*)")]
        [InlineData(@"(args @* @i)")]
        public void OneIntOneStar(string expression)
        {
            //arrange
            var oneIntMethod = ArgsMethods.First(m => m.Name == "OneIntOneGuid");

            //act
            var func = Compiler.Compile(new PointcutExpression(expression, oneIntMethod), null);

            var result = _module.FilterModule(func);

            //assert
            Assert.Contains(result, m => m.Name == "OneIntOneGuid" && m.DeclaringType.Name == "ArgsMethods");
            Assert.Contains(result, m => m.Name == "TwoInt" && m.DeclaringType.Name == "ArgsMethods");
        }
    }
}