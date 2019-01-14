using Mono.Cecil;

namespace Demon.Fody.Data
{
    //todo define data structure that holds regexes
    public class AdviceData
    {
        public AdviceData(MethodDefinition method, string pointCut, AdviceType type) => 
            (Method, PointCut, Type) = (method, pointCut, type);

        public MethodDefinition Method { get; }
        public string PointCut { get; }       
        public AdviceType Type { get; }
    }
}