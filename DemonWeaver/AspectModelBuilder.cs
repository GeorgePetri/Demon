using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemonWeaver.Data;
using DemonWeaver.ExpressionCompiler;
using DemonWeaver.ExpressionCompiler.Data;
using Mono.Cecil;
using Environment = DemonWeaver.ExpressionCompiler.Data.Environment;

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
                if (type.CustomAttributes.All(a => a.AttributeType.FullName != DemonTypes.FullNames.AspectAttribute))
                    return;

                foreach (var method in type.Methods)
                    AddMethodToDictionariesIfNeeded(method);
            });
        }

        //todo this is kinda copy paste, fix 
        //todo transforms to kebab case
        void AddMethodToDictionariesIfNeeded(MethodDefinition method)
        {
            foreach (var attribute in method.CustomAttributes)
            {
                if (attribute.AttributeType.FullName == DemonTypes.FullNames.PointcutAttribute)
                {
                    var pointcutExpression = (string) attribute.ConstructorArguments[0].Value;

                    var addedSuccessfully = _pointcutDefinitions.TryAdd(method.Name, new PointcutExpression(pointcutExpression, method));

                    if (!addedSuccessfully)
                        throw new WeavingException($"More than one pointcut with the name {method.Name} found.");

                    break;
                }

                if (attribute.AttributeType.FullName == DemonTypes.FullNames.BeforeAttribute)
                {
                    var pointcutExpression = (string) attribute.ConstructorArguments[0].Value;
                    _beforeDefinitions.Add(new PointcutExpression(pointcutExpression, method));

                    break;
                }

                if (attribute.AttributeType.FullName == DemonTypes.FullNames.AroundAttribute)
                {
                    var pointcutExpression = (string) attribute.ConstructorArguments[0].Value;
                    _aroundDefinitions.Add(new PointcutExpression(pointcutExpression, method));

                    break;
                }
            }
        }

        List<AdviceModel> ProcessAdvice()
        {
            var environment = new Environment(_pointcutDefinitions);

            IEnumerable<AdviceModel> CompileAdvice
            (
                IEnumerable<PointcutExpression> definitions,
                Func<PointcutExpression, Func<MethodDefinition, bool>, AdviceModel> func
            ) =>
                definitions
                    .Select(d =>
                    {
                        try
                        {
                            var compiledFunc = Compiler.Compile(d, environment);
                            return func(d, compiledFunc);
                        }
                        catch (WeavingException e)
                        {
                            throw new WeavingException($"{e.Message}At {d.DefiningMethod.FullName}");
                        }
                    });

            var beforeResults = CompileAdvice(_beforeDefinitions, (expression, func) => new BeforeAdvice(expression.DefiningMethod, func));

            var aroundResults = CompileAdvice(_aroundDefinitions, (expression, func) => new AroundAdvice(expression.DefiningMethod, func));

            return beforeResults.Concat(aroundResults)
                .ToList();
        }
    }
}