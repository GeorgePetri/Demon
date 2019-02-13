using System;
using TestsWeaving.Helpers;
using Xunit;

namespace TestsWeaving
{
    //todo add tests for complex types both null and non null 
    public class ArgsTests : IClassFixture<WeavedInMemoryModule>
    {
        readonly WeavedInMemoryModule _fixture;

        readonly dynamic _aspect;
        readonly dynamic _sut;

        public ArgsTests(WeavedInMemoryModule fixture)
        {
            _fixture = fixture;

            var type = _fixture.Assembly.GetType("TestDataForWeaving.Args.ArgsTarget");
            var aspectType = _fixture.Assembly.GetType("TestDataForWeaving.Args.ArgsAspect");

            _aspect = Activator.CreateInstance(aspectType);

            _sut = Activator.CreateInstance(type, _aspect);
        }

        [Fact]
        public void SingleInt()
        {
            _sut.TargetInt(5);

            //assert
            Assert.Equal(5, _aspect.LastBoundInt);
        }

        [Fact]
        public void Empty()
        {
            //act
            _sut.TargetEmpty();

            //assert
            Assert.True(_aspect.EmptyCalled);
        }

        [Fact]
        public void OptionalString_WhenNotNull()
        {
            //act
            _sut.TargetIntAndString(5, "five");

            //assert
            Assert.Equal("five", _aspect.LastBoundString);
        }

        [Fact]
        public void OptionalString_WhenNull()
        {
            //act
            _sut.TargetEmpty();

            //assert
            Assert.Null(_aspect.LastBoundString);
        }
    }
}