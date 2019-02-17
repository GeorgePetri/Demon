using System;
using TestsWeaving.Helpers;
using Xunit;

namespace TestsWeaving
{
    //todo add tests for static aspects, for static targets, for static targets with static constructors, for instance targets with static constructors, for multiple typejoinpoints in a type
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
            Assert.Equal(@"System.String Empty()", _aspect.BoundTypeJoinPoint.Method.ToString());
        }
    }
}