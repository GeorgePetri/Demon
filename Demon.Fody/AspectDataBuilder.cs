using System.Collections.Generic;
using System.Linq;
using Demon.Fody.Data;
using Mono.Cecil;

namespace Demon.Fody
{
    //todo impl ordering for aspects
    public class AspectDataBuilder
    {
        public static List<AspectData> FromTypeDefinition(IEnumerable<TypeDefinition> types) =>
            types.Where(t => t.CustomAttributes.Any(a => a.AttributeType.FullName == "Demon.Aspect.AspectAttribute"))
                .Select(MakeAspect)
                .ToList();

        private static AspectData MakeAspect(TypeDefinition type) => 
            new AspectData(type, MakeAdvice(type.Methods).ToList());

        private static IEnumerable<AdviceData> MakeAdvice(IEnumerable<MethodDefinition> methods) =>
            from method in methods
            from attribute in method.CustomAttributes
            let before = TryMakeBefore(attribute, method)
            let around = TryMakeAround(attribute, method)
            where before != null || around != null
            let all = new List<AdviceData> {before, around}
            from data in all
            where data != null
            select data;

        private static AdviceData TryMakeBefore(CustomAttribute attribute, MethodDefinition method) =>
            attribute.AttributeType.FullName == "Demon.Aspect.BeforeAttribute"
                ? new AdviceData(method, (string) attribute.ConstructorArguments[0].Value, AdviceType.Before)
                : null;

        private static AdviceData TryMakeAround(CustomAttribute attribute, MethodDefinition method) =>
            attribute.AttributeType.FullName == "Demon.Aspect.AroundAttribute"
                ? new AdviceData(method, (string) attribute.ConstructorArguments[0].Value, AdviceType.Around)
                : null;
    }
}