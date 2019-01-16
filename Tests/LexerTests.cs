using System.Text.RegularExpressions;
using Demon.Fody.PointcutExpression;
using Xunit;

namespace Tests
{
    //todo test proper class, rename
    public class LexerTests
    {
        [Fact]
        public void Complex()
        {
            //arrange
            const string pointcut = @"Repositories() !Get() && Execution(* **(Int,*)) Within(AssemblyToProcess.Repositories.**) || &&";

            var regex = new Regex(@"(?>&&|\|\||!|Execution\([^()]+\([^()]+\)\s*\)|Within\([^()]+\)|[a-zA-Z0-9]+\(\))", RegexOptions.Compiled);

            //act
            var match = regex.Matches(pointcut);

            //assert
            Assert.Equal(@"Repositories()",match[0].Value);
            Assert.Equal(@"!",match[1].Value);
            Assert.Equal(@"Get()",match[2].Value);
            Assert.Equal(@"&&",match[3].Value);
            Assert.Equal(@"Execution(* **(Int,*))",match[4].Value);
            Assert.Equal(@"Within(AssemblyToProcess.Repositories.**)",match[5].Value);
            Assert.Equal(@"||",match[6].Value);
            Assert.Equal(@"&&",match[7].Value);
        }

        //todo hacky
        [Fact]
        public void Wip_ProcessWithin()
        {
            var r = RegexFactory.TryProcessWithin("Within(*Aa.**  )");

            var x = 0;
        }
    }
}