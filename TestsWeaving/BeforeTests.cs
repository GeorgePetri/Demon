using System.Reflection;
using TestsWeaving.Helpers;
using Xunit;

namespace TestsWeaving
{
    //todo test static target, instance aspect
    //todo test instance target, static aspect
    //todo test instance target, instance aspect
    //todo test composition of aspects in other class
    [Collection(WeavedInMemoryModuleTestCollection.Name)]
    public class BeforeTests
    {
        readonly Assembly _assembly;

        public BeforeTests(WeavedInMemoryModule fixture) => _assembly = fixture.Assembly;

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