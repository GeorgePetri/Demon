using System.Collections.Generic;
using Demon.Fody.Data;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Demon.Fody
{
    public class TypeWeaver
    {
        readonly TypeDefinition _type;
        readonly List<AdviceModel> _adviceModels;

        public TypeWeaver(TypeDefinition type, List<AdviceModel> adviceModels) =>
            (_type, _adviceModels) = (type, adviceModels);

        //todo
        public static void Weave(TypeDefinition type, List<AdviceModel> aspects) =>
            new TypeWeaver(type, aspects).Weave();

        void Weave()
        {
            //todo add instance aspects when needed
            foreach (var method in _type.Methods)
            {
                foreach (var advice in _adviceModels)
                {
                    if (advice.FilterToApply(method) && DefaultFilter(method))
                        ApplyAdvice(method, advice);
                }
            }
        }

        static bool DefaultFilter(MethodDefinition method) =>
            method.HasBody
            && method.IsPublic
            && !method.IsConstructor;

        //todo hardcoded to before advice
        static void ApplyAdvice(MethodDefinition method, AdviceModel advice)
        {
            if (advice.Method.IsStatic)
                WeaveStatic(method, advice.Method);
            else
                WeaveInstance(method, advice.Method);
        }

        static void WeaveStatic(MethodDefinition target, MethodDefinition advice)
        {
            var il = target.Body.GetILProcessor();

            var callAdvice = il.Create(OpCodes.Call, advice);

            il.InsertBefore(target.Body.Instructions[0], callAdvice);
        }

        //todo support multiple public constructors use a dag and filter by :(this) calls
        static void WeaveInstance(MethodDefinition target, MethodDefinition advice)
        {
            //todo impl
        }
    }
}