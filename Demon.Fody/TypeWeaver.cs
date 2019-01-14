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

        private readonly Regex _executingRegex = new Regex(@"execution\((\*|\*\*|[a-zA-Z_.<>-]+)\s+([a-zA-Z_.<>-]+)\(([a-zA-Z_.<>\s-]+|\*|\**)\)\)", RegexOptions.Compiled);

        public TypeWeaver(TypeDefinition type, IEnumerable<AspectData> aspects) =>
            (_type, _aspects) = (type, aspects);

        //todo
        public static void Weave(TypeDefinition type, IEnumerable<AspectData> aspects) =>
            new TypeWeaver(type, aspects).Weave();

        //todo
        //todo https://docs.microsoft.com/en-us/dotnet/standard/base-types/best-practices
        private void Weave()
        {
            //todo don't foreach 3 times
            //todo don't match here
            foreach (var method in _type.Methods)
            {
                foreach (var aspect in _aspects)
                {
                    foreach (var advice in aspect.Advice)
                    {
                        if (ShouldApplyExecuting(method, advice.PointCut))
                            ApplyAdvice(method, advice);
                    }
                }
            }
        }

        //todo handle nonwildcard typename and parameters
        private bool ShouldApplyExecuting(MethodDefinition method, string pointcutExpression)
        {
            var match = _executingRegex.Match(pointcutExpression);

            if (!match.Success)
                return false;

            var typeName = match.Groups[1];
            var namespaceAndMethod = match.Groups[2];
            var paramaters = match.Groups[3];

            if ($"{method.DeclaringType.FullName}.{method.Name}" != namespaceAndMethod.Value)
                return false;

            if (typeName.Value != @"*" && typeName.Value != @"**")
                return false;
            
            if (paramaters.Value != @"*" && paramaters.Value != @"**")
                return false;

            return true;
        }

        private void ApplyAdvice(MethodDefinition method, AdviceData advice)
        {
            var x = 0;
        }
    }
}