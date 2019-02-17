using System;
using System.Reflection;
using TestsWeaving.Helpers;
using Xunit;

namespace TestsWeaving
{
    [Collection(WeavedInMemoryModuleTestCollection.Name)]
    public class ArgsTests
    {
        readonly dynamic _aspect;
        readonly dynamic _sut;
        readonly Assembly _assembly;

        public ArgsTests(WeavedInMemoryModule fixture)
        {
            _assembly = fixture.Assembly;
            
            var type = _assembly.GetType("TestDataForWeaving.Args.ArgsTarget");
            var aspectType = _assembly.GetType("TestDataForWeaving.Args.ArgsAspect");

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
        
        [Fact]
        public void OptionalComplex_WhenNotNull()
        {
            //arrange
            var complexClassType = _assembly.GetType("TestDataForWeaving.Args.ComplexClass");
            var complex = (dynamic)Activator.CreateInstance(complexClassType);
            
            //act
            _sut.TargetComplex(complex);

            //assert
            Assert.Equal(complex, _aspect.LastBoundComplex);
        }     
        
        [Fact]
        public void OptionalComplex_WhenNull()
        {
            //act
            _sut.TargetEmpty();

            //assert
            Assert.Null(_aspect.LastBoundComplex);
        }
    }
}