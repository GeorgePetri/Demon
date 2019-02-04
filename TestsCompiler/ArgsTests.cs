using DemonWeaver.PointcutExpressionCompiler;
using Mono.Cecil;
using Xunit;

namespace TestsCompiler
{
    public class ArgsTests
    {
        readonly ModuleDefinition _module = ModuleDefinition.ReadModule("TestDataForCompiler.dll");
        
        [Theory(Skip = "Not implemented")]
        [InlineData(@"Args()")]
        [InlineData(@"Args(  )")]
        public void Empty(string expression)
        {
            //act
            var func = Compiler.Compile(new PointcutExpression(expression, null), null);
        }
    }
}