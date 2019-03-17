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

        [Fact]
        void ParametersInt_ReturnString_JustProceed()
        {
            
        }
    }
}