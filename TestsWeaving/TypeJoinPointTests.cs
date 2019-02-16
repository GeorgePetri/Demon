using TestsWeaving.Helpers;
using Xunit;

namespace TestsWeaving
{
    [Collection(WeavedInMemoryModuleTestCollection.Name)]
    public class TypeJoinPointTests
    {
        readonly WeavedInMemoryModule _fixture;

        public TypeJoinPointTests(WeavedInMemoryModule fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        void InstanceAspect()
        {
            Assert.Equal(4, 2 + 2);
        }
    }
}