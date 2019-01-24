using System.Collections.Generic;
using Demon.Fody.Data;
using Fody;
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
                    if (advice.FilterToApply(method))
                        ApplyAdvice(method, advice);
                }
            }
        }

        //todo hardcoded to before advice
        void ApplyAdvice(MethodDefinition method, AdviceModel advice)
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
                throw new WeavingException(target.FullName + " does not have a body.");

            var il = target.Body.GetILProcessor();

            var callAdvice = il.Create(OpCodes.Call, advice);

            il.InsertBefore(target.Body.Instructions[0], callAdvice);
        }

        static void WeaveInstance(MethodDefinition target, MethodDefinition advice)
        {
            //todo replace body precondition to filtering methods without body when resolving pointcuts
            if (!target.HasBody)
                throw new WeavingException(target.FullName + " does not have a body.");

            //todo impl
        }
    }
}