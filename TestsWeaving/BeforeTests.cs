using System;
using System.Reflection;
using TestsWeaving.Helpers;
using Xunit;

namespace TestsWeaving
{
    //todo test static target, instance aspect
    //todo test instance target, static aspect
    //todo test composition of aspects in other class
    //todo idea, make these tests in dependency to test imports
    [Collection(WeavedInMemoryModuleTestCollection.Name)]
    public class BeforeTests
    {
        readonly Assembly _assembly;

        public BeforeTests(WeavedInMemoryModule fixture) => _assembly = fixture.Assembly;

        [Fact]
        public void InstanceTargetInstanceAspect()
        {
            //arrange
            var aspect = _assembly.GetType("TestDataForWeaving.Before.InstanceTargetInstanceAspect.Aspect");
            var aspectInstance = (dynamic) Activator.CreateInstance(aspect);

            var type = _assembly.GetType("TestDataForWeaving.Before.InstanceTargetInstanceAspect.Target");
            var typeInstance = (dynamic) Activator.CreateInstance(type, aspectInstance);

            //act
            typeInstance.OneInt(5);
            
            //assert
            Assert.True(aspectInstance.Called);
        }

        [Fact]
        public void StaticTargetStaticAspect()
        {
            //arrange
            var type = _assembly.GetType("TestDataForWeaving.Before.StaticTargetStaticAspect.Target");

            //assert
            var exception = Assert.Throws<TargetInvocationException>(() => type.GetMethod("OneInt", BindingFlags.Static | BindingFlags.Public)
                .Invoke(null, new object[] {5}));

            Assert.Equal("Weaved", exception.InnerException.Message);
        }
    }
}