using System.Collections.Generic;
using System.Text.RegularExpressions;
using Demon.Fody.Data;
using Mono.Cecil;

namespace Demon.Fody
{
    //todo this call does all the aspect weaving for a type
    public class TypeWeaver
    {
        private readonly TypeDefinition _type;
        private readonly IEnumerable<AspectData> _aspects;

        public TypeWeaver(TypeDefinition type, IEnumerable<AspectData> aspects) =>
            (_type, _aspects) = (type, aspects);

        //todo
        public static void Weave(TypeDefinition type, IEnumerable<AspectData> aspects) =>
            new TypeWeaver(type, aspects).Weave();

        //todo
        //todo https://docs.microsoft.com/en-us/dotnet/standard/base-types/best-practices
        private void Weave()
        {
            var s = "execution(void AssemblyToProcess.BeforeAdvice.Static.StaticBeforeTarget.Target(Int))";
            
            var executingRegex = new Regex(@"execution\((\*|\*\*|[a-zA-Z_.<>-]+)\s+([a-zA-Z_.<>-]+)\(([a-zA-Z_.<>\s-]+|\*|\**)\)\)", RegexOptions.Compiled);

            var m = executingRegex.Match(s);
            var type = m.Groups[1];
            var namespac = m.Groups[2];
            var parameters = m.Groups[3];
        }
    }
}