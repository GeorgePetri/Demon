using System;
using System.Reflection;
using TestsWeaving.Helpers;
using Xunit;

namespace TestsWeaving
{
    //todo test composition of aspects in other class
    //todo idea, make these tests in dependency to test imports
    //todo StaticTargetInstanceAspect should not bind, fix in typeweaver
    [Collection(WeavedInMemoryModuleTestCollection.Name)]
    public class BeforeTests
    {
        readonly Assembly _assembly;

        public BeforeTests(WeavedInMemoryModule fixture) => _assembly = fixture.Assembly;

        [Fact]
        public void InstanceTargetInstanceAspect()
        {
            //arrange
            var aspectType = _assembly.GetType("TestDataForWeaving.Before.InstanceTargetInstanceAspect.Aspect");
            var aspectInstance = (dynamic) Activator.CreateInstance(aspectType);

            var targetType = _assembly.GetType("TestDataForWeaving.Before.InstanceTargetInstanceAspect.Target");
            var targetInstance = (dynamic) Activator.CreateInstance(targetType, aspectInstance);

            //act
            targetInstance.OneInt(5);

            //assert
            Assert.True(aspectInstance.Called);
        }

        [Fact]
        public void InstanceTargetStaticAspect()
        {
            //arrange
            var targetType = _assembly.GetType("TestDataForWeaving.Before.InstanceTargetStaticAspect.Target");
            var targetInstance = (dynamic) Activator.CreateInstance(targetType);

            //assert
            var exception = Assert.Throws<Exception>(() => targetInstance.OneInt(5));

            Assert.Equal("Weaved", exception.Message);
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