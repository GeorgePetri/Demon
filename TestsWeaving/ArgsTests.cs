using System;
using System.Reflection;
using TestsWeaving.Helpers;
using Xunit;

namespace TestsWeaving
{
    //todo uncopypaste
    public class ArgsTests : IClassFixture<WeavedInMemoryModule>
    {
        readonly WeavedInMemoryModule _fixture;
        readonly Assembly _assembly;

        public ArgsTests(WeavedInMemoryModule fixture) => (_fixture, _assembly) = (fixture, fixture.Assembly);

        [Fact]
        public void SingleInt()
        {
            //arrange
            var type = _assembly.GetType("TestDataForWeaving.Args.ArgsTarget");
            var aspectType = _assembly.GetType("TestDataForWeaving.Args.ArgsAspect");

            var aspect = (dynamic) Activator.CreateInstance(aspectType);

            var instance = Activator.CreateInstance(type, aspect);

            //act
            instance.TargetInt(5);

            //assert
            Assert.Equal(5, aspect.LastBoundInt);
        }

        [Fact]
        public void Empty()
        {
            //arrange
            var type = _assembly.GetType("TestDataForWeaving.Args.ArgsTarget");
            var aspectType = _assembly.GetType("TestDataForWeaving.Args.ArgsAspect");

            var aspect = (dynamic) Activator.CreateInstance(aspectType);

            var instance = Activator.CreateInstance(type, aspect);

            //act
            instance.TargetEmpty();

            //assert
            Assert.True(aspect.EmptyCalled);
        }
    }
}