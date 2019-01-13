using System.Collections.Generic;
using Mono.Cecil;

namespace Demon.Fody.Data
{
    public class AspectData
    {
        public AspectData(TypeDefinition aspect, IReadOnlyList<AdviceData> advices) => 
            (Aspect, Advice) = (aspect,advices);

        public TypeDefinition Aspect { get; }

        public IReadOnlyList<AdviceData> Advice { get; }
    }
}