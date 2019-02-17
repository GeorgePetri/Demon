using System;
using System.Reflection;
using Xunit;

#pragma warning disable 618

namespace Tests
{
    //todo test static targets
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

        [Fact]
        public void InstanceAdvice()
        {
            //arrange
            var type = _assembly.GetType("AssemblyToProcess.BeforeAdvice.Instance.InstanceBeforeTarget");
            var aspectType = _assembly.GetType("AssemblyToProcess.BeforeAdvice.Instance.StatefulInstanceAspect");

            var aspect = (dynamic) Activator.CreateInstance(aspectType);

            var instance = Activator.CreateInstance(type, aspect);

            //act
            instance.Target(5);

            //assert
            Assert.True(aspect.AdviceCalled);
        }

        [Fact]
        public void InstanceAdviceManyConstructorParameters()
        {
            //arrange
            var type = _assembly.GetType("AssemblyToProcess.BeforeAdvice.Instance.InstanceBeforeTarget2");
            var aspectType = _assembly.GetType("AssemblyToProcess.BeforeAdvice.Instance.StatefulInstanceAspect");

            var aspect = (dynamic) Activator.CreateInstance(aspectType);

            var instance = Activator.CreateInstance(type, "test", 1, 2, 3, 4, 5, aspect);

            //act
            instance.Target(5);

            //assert
            Assert.True(aspect.AdviceCalled);
        }
    }
}