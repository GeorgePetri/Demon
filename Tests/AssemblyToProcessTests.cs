using System;
using Demon.Fody;
using Fody;
using Xunit;

#pragma warning disable 618

namespace Tests
{
    //todo split weaving tests in another project
    public class AssemblyToProcessTests
    {
        private readonly TestResult _result;

        //todo add assemblies for testing weave time validation?
        public AssemblyToProcessTests()
        {
            var weaver = new ModuleWeaver();

            _result = weaver.ExecuteTestRun("AssemblyToProcess.dll");
        }

        [Fact]
        public void StaticAdvice()
        {
            var type = _result.Assembly.GetType("AssemblyToProcess.BeforeAdvice.Static.StaticBeforeTarget");

            var instance = (dynamic)Activator.CreateInstance(type);

            Assert.Throws<ApplicationException>(() => instance.Target(5));
        }
    }
}