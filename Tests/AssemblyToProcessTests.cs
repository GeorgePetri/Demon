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
        readonly TestResult _result;

        //todo add assemblies for testing weave time validation?
        public AssemblyToProcessTests()
        {
            var weaver = new ModuleWeaver();

            _result = weaver.ExecuteTestRun("AssemblyToProcess.dll");
        }

        [Fact]
        public void StaticAdvice()
        {
            //arrange
            var type = _result.Assembly.GetType("AssemblyToProcess.BeforeAdvice.Static.StaticBeforeTarget");

            var instance = (dynamic) Activator.CreateInstance(type);

            //assert
            Assert.Throws<ApplicationException>(() => instance.Target(5));
        }

        //todo check if works with ms di
        [Fact]
        public void InstanceAdvice()
        {
            //arrange
            var type = _result.Assembly.GetType("AssemblyToProcess.BeforeAdvice.Instance.InstanceBeforeTarget");
            var aspectType = _result.Assembly.GetType("AssemblyToProcess.BeforeAdvice.Instance.StatefulInstanceAspect");

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
            var type = _result.Assembly.GetType("AssemblyToProcess.BeforeAdvice.Instance.InstanceBeforeTarget2");
            var aspectType = _result.Assembly.GetType("AssemblyToProcess.BeforeAdvice.Instance.StatefulInstanceAspect");

            var aspect = (dynamic) Activator.CreateInstance(aspectType);

            var instance = Activator.CreateInstance(type, "test", 1, 2, 3, 4, 5, aspect);

            //act
            instance.Target(5);

            //assert
            Assert.True(aspect.AdviceCalled);
        }
    }
}