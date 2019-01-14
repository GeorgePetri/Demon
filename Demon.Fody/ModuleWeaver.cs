using System.Collections.Generic;
using System.Linq;
using Fody;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Demon.Fody
{
    public class ModuleWeaver : BaseModuleWeaver
    {
        //todo optimize ils, both manually and with cecil function
        public override void Execute()
        {
//            var aspects = GetAspects();
//
//            foreach (var aspect in aspects)
//            {
//                var advice = GetBefore(aspect);
//
//                foreach (var (adviceMethod, pointCutExpression) in advice)
//                {
//                    var target = ResolvePointCut(pointCutExpression);
//
//                    if (adviceMethod.IsStatic)
//                        WeaveStatic(adviceMethod, target);
//                    else
//                        WeaveInstance(adviceMethod, target);
//                }
//            }

            //todo remove above code

            var aspectsX = AspectDataBuilder.FromTypeDefinitions(ModuleDefinition.Types);

            //todo filter to not run on aspects accidentally
            foreach (var type in ModuleDefinition.Types)
                TypeWeaver.Weave(type, aspectsX);
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
        private MethodDefinition ResolvePointCut(string expression)
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
        private MethodDefinition ResolveMethodByNamespace(string expression)
        {
            var trimmed = expression.Trim();

            var split = trimmed.Split('.');

            var methodName = split.Last();

            var typeFullName = trimmed.Substring(0, trimmed.Length - methodName.Length - 1);

            var type = ModuleDefinition.Types
                           .FirstOrDefault(t => t.FullName == typeFullName)
                       ?? throw new WeavingException(expression + " type not found");

            return type.Methods
                       .FirstOrDefault(m => m.Name == methodName)
                   ?? throw new WeavingException(expression + " method not found");
        }

        private static void WeaveStatic(MethodDefinition advice, MethodDefinition target)
        {
            //todo replace body precondition to filtering methods without body when resolving pointcuts
            if (!target.HasBody)
                throw new WeavingException(target.FullName + " does not have a body");

            var il = target.Body.GetILProcessor();

            var callAdvice = il.Create(OpCodes.Call, advice);

            il.InsertBefore(target.Body.Instructions[0], callAdvice);
        }

        private static void WeaveInstance(MethodDefinition advice, MethodDefinition target)
        {
            //todo replace body precondition to filtering methods without body when resolving pointcuts
            if (!target.HasBody)
                throw new WeavingException(target.FullName + " does not have a body");

            //todo impl
        }
    }
}