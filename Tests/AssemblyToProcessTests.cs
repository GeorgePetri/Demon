using System;
using System.Reflection;
using Xunit;

#pragma warning disable 618

namespace Tests
{
    //todo split weaving tests in another project
    public class AssemblyToProcessTests
    {
        readonly Assembly _assembly;

        //todo add assemblies for testing weave time validation?
        public AssemblyToProcessTests()
        {
            _assembly = Assembly.Load("AssemblyToProcess");
        }

        [Fact]
        public void StaticAdvice()
        {
            //arrange
            var type = _assembly.GetType("AssemblyToProcess.BeforeAdvice.Static.StaticBeforeTarget");

            var instance = (dynamic) Activator.CreateInstance(type);

            //assert
            Assert.Throws<ApplicationException>(() => instance.Target(5));
        }
    }
}