using System;
using TestsWeaving.Helpers;
using Xunit;

namespace TestsWeaving
{
    //todo add tests for for multiple typejoinpoints in a type
    [Collection(WeavedInMemoryModuleTestCollection.Name)]
    public class TypeJoinPointTests
    {
        readonly dynamic _aspect;
        readonly dynamic _sut;

        public TypeJoinPointTests(WeavedInMemoryModule fixture)
        {
            var type = fixture.Assembly.GetType("TestDataForWeaving.TypeJoinPoint.Target");
            var aspectType = fixture.Assembly.GetType("TestDataForWeaving.TypeJoinPoint.InstanceAspect");

            _aspect = Activator.CreateInstance(aspectType);

            _sut = Activator.CreateInstance(type, _aspect);
        }

        [Fact]
        void InstanceAspect()
        {
            //act
            _sut.Empty();
            
            //assert
            Assert.Equal(@"System.String TestDataForWeaving.TypeJoinPoint.Target::Empty()", _aspect.BoundTypeJoinPoint.FullName);
        }
    }
}