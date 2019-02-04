using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemonWeaver.Data;
using DemonWeaver.PointcutExpressionCompiler;
using Mono.Cecil;

namespace DemonWeaver
{
    //todo impl ordering
    //todo test paralelize
    //todo test this class
    public class AspectModelBuilder
    {
        readonly ConcurrentDictionary<string, PointcutExpression> _pointcutDefinitions = new ConcurrentDictionary<string, PointcutExpression>();
        readonly ConcurrentBag<PointcutExpression> _beforeDefinitions = new ConcurrentBag<PointcutExpression>();
        readonly ConcurrentBag<PointcutExpression> _aroundDefinitions = new ConcurrentBag<PointcutExpression>();

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

                    var addedSuccessfully = _pointcutDefinitions.TryAdd(method.Name, new PointcutExpression(pointcutExpression, method));

                    if (!addedSuccessfully)
                        throw new WeavingException($"More than one pointcut with the name{method.Name} found");

                    break;
                }

                if (attribute.AttributeType.FullName == "Demon.Aspect.BeforeAttribute")
                {
                    var pointcutExpression = (string) attribute.ConstructorArguments[0].Value;
                    _beforeDefinitions.Add(new PointcutExpression(pointcutExpression, method));

                    break;
                }

                if (attribute.AttributeType.FullName == "Demon.Aspect.AroundAttribute")
                {
                    var pointcutExpression = (string) attribute.ConstructorArguments[0].Value;
                    _aroundDefinitions.Add(new PointcutExpression(pointcutExpression, method));

                    break;
                }
            }
        }

        //todo catch weaving exceptions and add contextual information such as pointcut string and aspect
        List<AdviceModel> ProcessAdvice()
        {
            var pointcutContext = new PointcutContext(_pointcutDefinitions);

            var beforeResults = from definition in _beforeDefinitions
                let function = Compiler.Compile(definition, pointcutContext)
                select new BeforeAdvice(definition.DefiningMethod, function) as AdviceModel;

            var aroundResults = from definition in _aroundDefinitions
                let function = Compiler.Compile(definition, pointcutContext)
                select new AroundAdvice(definition.DefiningMethod, function) as AdviceModel;

            return beforeResults.Concat(aroundResults)
                .ToList();
        }
    }
}