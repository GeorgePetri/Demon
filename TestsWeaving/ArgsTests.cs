using Mono.Cecil;
using Xunit;
using static TestsWeaving.Helpers.Helpers;

namespace TestsWeaving
{
    public class ArgsTests
    {
        readonly ModuleDefinition _module = ModuleDefinition.ReadModule(TestDataFilename);

        [Fact]
        public void SingleInt()
        {
            var x = 0;

//
//                //arrange
//                var type = _assembly.GetType("AssemblyToProcess.BeforeAdvice.Args.ArgsTarget");
//                var aspectType = _assembly.GetType("AssemblyToProcess.BeforeAdvice.Args.ArgsAspect");
//
//                var aspect = (dynamic) Activator.CreateInstance(aspectType);
//
//                var instance = Activator.CreateInstance(type, aspect);
//
//                //act
//                instance.TargetInt(5);
//
//                //assert
//                Assert.Equal(5,aspect.LastBoundInt);
        }
    }
}