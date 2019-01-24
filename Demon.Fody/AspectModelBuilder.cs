using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demon.Fody.Data;
using Demon.Fody.PointcutExpression;
using Fody;
using Mono.Cecil;

namespace Demon.Fody
{
    //todo impl ordering
    //todo test paralelize
    //todo test this class
    public class AspectModelBuilder
    {
        readonly ConcurrentDictionary<string, string> _pointcutDefinitions = new ConcurrentDictionary<string, string>();
        readonly ConcurrentDictionary<MethodDefinition, string> _beforeDefinitions = new ConcurrentDictionary<MethodDefinition, string>();
        readonly ConcurrentDictionary<MethodDefinition, string> _aroundDefinitions = new ConcurrentDictionary<MethodDefinition, string>();

        public static List<AdviceModel> FromTypeDefinitions(IEnumerable<TypeDefinition> types)
        {
            var instance = new AspectModelBuilder();

            instance.FillDictionaries(types);

            return instance.ProcessAdvice();
        }

        void FillDictionaries(IEnumerable<TypeDefinition> types)
        {
            Parallel.ForEach(types, type =>
            {
                if (type.CustomAttributes.All(a => a.AttributeType.FullName != "Demon.Aspect.AspectAttribute"))
                    return;

                foreach (var method in type.Methods)
                    AddMethodToDictionariesIfNeeded(method);
            });
        }

        //todo this and ProcessAdvice are kinda copy paste, fix 
        void AddMethodToDictionariesIfNeeded(MethodDefinition method)
        {
            foreach (var attribute in method.CustomAttributes)
            {
                if (attribute.AttributeType.FullName == "Demon.Aspect.PointcutAttribute")
                {
                    var pointcutExpression = (string) attribute.ConstructorArguments[0].Value;

                    var addedSuccessfully = _pointcutDefinitions.TryAdd(method.Name, pointcutExpression);

                    if (!addedSuccessfully)
                        throw new WeavingException($"More than one pointcut with the name{method.Name} found");

                    break;
                }

                if (attribute.AttributeType.FullName == "Demon.Aspect.BeforeAttribute")
                {
                    var pointcutExpression = (string) attribute.ConstructorArguments[0].Value;
                    _beforeDefinitions.TryAdd(method, pointcutExpression);

                    break;
                }

                if (attribute.AttributeType.FullName == "Demon.Aspect.AroundAttribute")
                {
                    var pointcutExpression = (string) attribute.ConstructorArguments[0].Value;
                    _aroundDefinitions.TryAdd(method, pointcutExpression);

                    break;
                }
            }
        }

        //todo catch weaving exceptions and add contextual information such as pointcut string and aspect
        List<AdviceModel> ProcessAdvice()
        {
            var pointcutContext = new PointcutContext(_pointcutDefinitions);

            var beforeResults = from definition in _beforeDefinitions
                let method = definition.Key
                let expression = definition.Value
                let function = Compiler.Compile(expression, pointcutContext)
                select new BeforeAdvice(method, function) as AdviceModel;

            var aroundResults = from definition in _aroundDefinitions
                let method = definition.Key
                let expression = definition.Value
                let function = Compiler.Compile(expression, pointcutContext)
                select new AroundAdvice(method, function) as AdviceModel;

            return beforeResults.Concat(aroundResults)
                .ToList();
        }
    }
}