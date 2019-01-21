using Mono.Cecil;

namespace Demon.Fody.Data
{
    //todo defineor update this  data structure that holds delegate
    //todo try get rid of enum
    public class AdviceData
    {
        public AdviceData(MethodDefinition method, string pointCut, AdviceType type) => 
            (Method, PointCut, Type) = (method, pointCut, type);

        public MethodDefinition Method { get; }
        public string PointCut { get; }       
        public AdviceType Type { get; }
    }
}