using System.Collections.Generic;
using System.Text.RegularExpressions;
using Demon.Fody.Data;
using Fody;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Demon.Fody
{
    //todo add validation to grammar tokens like execution and within etc
    public class TypeWeaver
    {
        readonly TypeDefinition _type;
        readonly IEnumerable<AspectData> _aspects;

        //todo dsaible backtracking, validate whitepsaces
        readonly Regex _executingRegex = new Regex(@"execution\((\*|\*\*|[a-zA-Z_.<>-]+)\s+([a-zA-Z_.<>-]+)\(([a-zA-Z_.<>\s-]+|\*|\**)\)\)", RegexOptions.Compiled);

        public TypeWeaver(TypeDefinition type, IEnumerable<AspectData> aspects) =>
            (_type, _aspects) = (type, aspects);

        //todo
        public static void Weave(TypeDefinition type, IEnumerable<AspectData> aspects) =>
            new TypeWeaver(type, aspects).Weave();

        //todo
        void Weave()
        {
            //todo don't foreach 3 times
            //todo don't match here
            //todo add instance aspects when needed
            foreach (var method in _type.Methods)
            {
                foreach (var aspect in _aspects)
                {
                    foreach (var advice in aspect.Advice)
                    {
                        if (ShouldApplyExecuting(method, advice.PointCut))
                            ApplyAdvice(method, advice);
                    }
                }
            }
        }

        //todo handle nonwildcard typename and parameters
        bool ShouldApplyExecuting(MethodDefinition method, string pointcutExpression)
        {
            var match = _executingRegex.Match(pointcutExpression);

            if (!match.Success)
                return false;

            var typeName = match.Groups[1];
            var namespaceAndMethod = match.Groups[2];
            var parameters = match.Groups[3];

            if ($"{method.DeclaringType.FullName}.{method.Name}" != namespaceAndMethod.Value)
                return false;

            if (typeName.Value != @"*" && typeName.Value != @"**")
                return false;

            if (parameters.Value != @"*" && parameters.Value != @"**")
                return false;

            return true;
        }

        //todo hardcoded to before advice
        void ApplyAdvice(MethodDefinition method, AdviceData advice)
        {
            if (advice.Method.IsStatic)
                WeaveStatic(method, advice.Method);
            else
                WeaveInstance(method, advice.Method);
        }

        static void WeaveStatic(MethodDefinition target, MethodDefinition advice)
        {
            //todo replace body precondition to filtering methods without body when resolving pointcuts
            if (!target.HasBody)
                throw new WeavingException(target.FullName + " does not have a body");

            var il = target.Body.GetILProcessor();

            var callAdvice = il.Create(OpCodes.Call, advice);

            il.InsertBefore(target.Body.Instructions[0], callAdvice);
        }

        static void WeaveInstance(MethodDefinition target, MethodDefinition advice)
        {
            //todo replace body precondition to filtering methods without body when resolving pointcuts
            if (!target.HasBody)
                throw new WeavingException(target.FullName + " does not have a body");

            //todo impl
        }
    }
}