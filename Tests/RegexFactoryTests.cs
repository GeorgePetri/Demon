using Demon.Fody.PointcutExpression;
using Xunit;

namespace Tests
{
    public class RegexFactoryTests
    {
        [Theory]
        [InlineData(@"")]
        [InlineData(@"Executing(* **(**)")]
        [InlineData(@"Within()")]
        [InlineData(@"Within(-)")]
        [InlineData(@"Within(_)")]
        [InlineData(@"Within(=)")]
        public void TryProcessWithin_ReturnsNull_WhenNotMatching(string token)
        {
            //act
            var result = RegexFactory.TryProcessWithin(token);

            //assert
            Assert.Null(result);
        }       
        
        [Theory]
        [InlineData(@"Within(Assembly.Folder.Class)")]
        [InlineData(@"Within( Assembly.Folder.Class)")]
        [InlineData(@"Within(Assembly.Folder.Class )")]
        [InlineData(@"Within(   Assembly.Folder.Class)")]
        [InlineData(@"Within(Assembly.Folder.Class   )")]
        [InlineData(@"Within(   Assembly.Folder.Class   )")]
        public void TryProcessWithin_ReturnsRegex_WhenMatching_NoStars_VariousWhitespaces(string token)
        {
            //act
            var result = RegexFactory.TryProcessWithin(token);

            //assert           
            Assert.Equal(@"Assembly\.Folder\.Class",result.ToString());
        }
        
        [Fact]
        public void TryProcessWithin_ReturnsRegex_WhenMatching_SingleStar()
        {
            //arrange
            const string token = @"Within(Assembly.Folder.*Class)";
            
            //act
            var result = RegexFactory.TryProcessWithin(token);

            //assert           
            Assert.Equal(@"Assembly\.Folder\.[a-zA-Z1-9]+Class",result.ToString());
        }
        
        [Fact]
        public void TryProcessWithin_ReturnsRegex_WhenMatching_DoubleStar()
        {
            //arrange
            const string token = @"Within( **.Class)";
            
            //act
            var result = RegexFactory.TryProcessWithin(token);

            //assert           
            Assert.Equal(@"[a-zA-Z1-9.]+\.Class",result.ToString());
        }
    }
}