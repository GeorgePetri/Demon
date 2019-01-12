using System.Collections.Generic;
using System.Linq;
using Fody;
using Mono.Cecil;

namespace Demon.Fody
{
    public class ModuleWeaver : BaseModuleWeaver
    {
        public override void Execute()
        {
            var aspects = GetAspects();

            foreach (var aspect in aspects)
            {
                var advice = GetBefore(aspect);

                foreach (var (adviceMethod, pointCutExpression) in advice)
                {
                    ResolvePointCut(pointCutExpression);
                }
            }

            var objectType = FindType("System.Object");
            var objectImport = ModuleDefinition.ImportReference(objectType);
            ModuleDefinition.Types.Add(new TypeDefinition("MyNamespace", "MyType", TypeAttributes.Public,
                objectImport));
        }

        public override IEnumerable<string> GetAssembliesForScanning()
        {
            //todo add mscorlib?
            yield break;
        }

        public override bool ShouldCleanReference { get; } = false;

        private IEnumerable<TypeDefinition> GetAspects() =>
            ModuleDefinition.Types
                .Where(t => t.CustomAttributes
                    .Any(a => a.AttributeType.FullName == "Demon.Aspect.AspectAttribute"));

        private static IEnumerable<(MethodDefinition method, string pointCutExpression)> GetBefore(TypeDefinition aspect) =>
            from method in aspect.Methods
            from attribute in method.CustomAttributes
            where attribute.AttributeType.FullName == "Demon.Aspect.BeforeAttribute"
            select (method, (string) attribute.ConstructorArguments[0].Value);

        //todo define grammar for pointcuts, only supports execution with no wildcards or types
        //todo can pointcuts be constructed nicer than strings that get parsed?
        //todo don't use exceptions for flow control
        private TypeDefinition ResolvePointCut(string expression)
        {
            var trimmedExpression = expression.Trim();
            const string executionString = "execution";
            if (trimmedExpression.StartsWith(executionString))
            {
                var withoutExecutionString = trimmedExpression.Substring(executionString.Length);
                var withoutParentheses = withoutExecutionString.Substring(1, withoutExecutionString.Length - 2); //todo handle parentheses properly
                return ResolveMethodByNamespace(withoutParentheses);
            }

            throw new WeavingException($"Only {executionString} is supported.");
        }

        //todo hacky
        //todo handle multiple
        //todo handle wildcards
        private TypeDefinition ResolveMethodByNamespace(string expression)
        {
            var trimmed = expression.Trim();

            var split = trimmed.Split('.');

            var methodName = split.Last();

            var typeFullName = trimmed.Substring(0, trimmed.Length - methodName.Length - 1);

            return ModuleDefinition.Types
                       .FirstOrDefault(t => t.FullName == typeFullName)
                   ?? throw new WeavingException(expression + " method not found");
        }
    }
}