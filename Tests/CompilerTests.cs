using System.Linq;
using Demon.Fody.PointcutExpression;
using Mono.Cecil;
using Xunit;

namespace Tests
{
    public class CompilerTests
    {
        private readonly ModuleDefinition _module = ModuleDefinition.ReadModule("TestDataForCompiler.dll");

        [Fact]
        public void Within_IsFalse_ForNotWithinTarget()
        {
            //arrange
            const string expression = @"Within(TestDataForCompiler.NotExisting)";

            var compiler = new Compiler(expression);

            //act
            var func = compiler.Compile();

            var result = _module.Types
                .SelectMany(t => t.Methods)
                .Where(func)
                .ToList();
            
            //assert
            Assert.False(result.Any());
        }
    }
}