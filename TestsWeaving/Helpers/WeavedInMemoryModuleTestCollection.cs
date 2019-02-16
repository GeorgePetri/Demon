using Xunit;

namespace TestsWeaving.Helpers
{
    [CollectionDefinition(Name)]
    public class WeavedInMemoryModuleTestCollection : ICollectionFixture<WeavedInMemoryModule>
    {
        public const string Name = "WeavedInMemoryModuleTestCollection";
    }
}