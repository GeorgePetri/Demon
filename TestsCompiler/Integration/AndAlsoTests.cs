using DemonWeaver;
using DemonWeaver.ExpressionCompiler;
using DemonWeaver.ExpressionCompiler.Data;
using Mono.Cecil;
using TestsCompiler.Helpers;
using Xunit;

namespace TestsCompiler.Integration
{
    //todo test variadic
    public class AndAlsoTests
    {
        readonly ModuleDefinition _module = ModuleDefinition.ReadModule("TestDataForCompiler.dll");

        [Theory]
        [InlineData(@"(and (within @TestDataForCompiler.Controllers.**) (within @**.Get))")]
        [InlineData(@"(and (within @TestDataForCompiler.Controllers.**) (not (within @**.Post)))")]
        public void Within(string expression)
        {
            //act
            var func = Compiler.Compile(new PointcutExpression(expression, null), null);

            var result = _module.FilterModule(func);

            //assert   
            Assert.Equal(2, result.Count);
            Assert.Equal("Get", result[0].Name);
            Assert.Equal("Get", result[1].Name);
        }

        [Theory]
        [InlineData(@"(and)")]
        [InlineData(@"(and (within @**.Get))")]
        public void Throws_IfNotAtLeast2Args(string expression)
        {
            //arrange
            var compiler = new Compiler(new PointcutExpression(expression, null), null);

            //assert   
            Assert.Throws<WeavingException>(() => compiler.Compile());
        }
    }
}