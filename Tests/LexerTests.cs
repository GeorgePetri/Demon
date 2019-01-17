using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Xunit;

namespace Tests
{
    //todo test proper class, rename
    public class LexerTests
    {
        //todo test an actual class
        [Fact]
        public void Complex()
        {
            //arrange
            const string pointcut = @"Repositories() !Get() && Execution(* **(Int,*)) Within(AssemblyToProcess.Repositories.**) || &&";

            var regex = new Regex(@"(?>&&|\|\||!|Execution\([^()]+\([^()]+\)\s*\)|Within\([^()]+\)|[a-zA-Z0-9]+\(\))", RegexOptions.Compiled);

            //act
            var match = regex.Matches(pointcut);

            //assert
            Assert.Equal(@"Repositories()", match[0].Value);
            Assert.Equal(@"!", match[1].Value);
            Assert.Equal(@"Get()", match[2].Value);
            Assert.Equal(@"&&", match[3].Value);
            Assert.Equal(@"Execution(* **(Int,*))", match[4].Value);
            Assert.Equal(@"Within(AssemblyToProcess.Repositories.**)", match[5].Value);
            Assert.Equal(@"||", match[6].Value);
            Assert.Equal(@"&&", match[7].Value);
        }

        [Fact]
        public void CompileBenchmark()
        {
            var regex = new Regex(@"(?>&&|\|\||!|Execution\([^()]+\([^()]+\)\s*\)|Within\([^()]+\)|[a-zA-Z0-9]+\(\))", RegexOptions.Compiled);

            var stopWatch = Stopwatch.StartNew();

            var parameter = Expression.Parameter(typeof(string));
            var constant = Expression.Constant(regex);

            var call = Expression.Call(
                constant,
                typeof(Regex)
                    .GetMethod(nameof(Regex.IsMatch), new[] {typeof(string)})
                , parameter);


            var e = Expression.Lambda<Func<string, bool>>(call, parameter);

            var d = e.Compile();

            var b = d("dwadawda");
            var b1 = d("&&");
            var b2 = d("dwadawdaaa");

            stopWatch.Stop();

            var x = stopWatch.Elapsed;
        }
    }
}