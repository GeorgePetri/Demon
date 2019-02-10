using Mono.Cecil;

namespace DemonWeaver
{
    public class MethodWeaver
    {
        readonly MethodDefinition _target;
        readonly MethodDefinition _advice;

        public MethodWeaver(MethodDefinition target, MethodDefinition advice) =>
            (_target, _advice) = (target, advice);

        public void Weave()
        {
            
        }
    }
}