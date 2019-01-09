using Demon.Fody;
using Fody;
using Xunit;

namespace Tests
{
    public class AssemblyToProcessTests
    {
        [Fact]
        public void Sanity()
        {
            Assert.Equal(4, 2 + 2);
        }

        [Fact]
        public void Weave()
        {
            var weaver = new ModuleWeaver();

            var result = weaver.ExecuteTestRun("AssemblyToProcess.dll");
        }
    }
}