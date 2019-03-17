using System;
using System.Reflection;
using TestsWeaving.Helpers;
using Xunit;

namespace TestsWeaving
{
    [Collection(WeavedInMemoryModuleTestCollection.Name)]
    public class AroundTests
    {
        readonly Assembly _assembly;

        public AroundTests(WeavedInMemoryModule fixture) => _assembly = fixture.Assembly;

        //also test return is correct
//        [Fact]
        void ParametersInt_ReturnString_JustProceed()
        {
            //arrange
            var aspectType = _assembly.GetType("TestDataForWeaving.Around.JustProceed.Aspect");
            var aspectInstance = (dynamic) Activator.CreateInstance(aspectType);

            var targetType = _assembly.GetType("TestDataForWeaving.Around.JustProceed.Target");
            var targetInstance = Activator.CreateInstance(targetType, aspectInstance);

            //act
            targetInstance.OneInt(5);
            
            //assert
            Assert.True(aspectInstance.Called);
        }
    }
}