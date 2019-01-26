using System.Collections.Generic;
using System.Linq;
using Demon.Fody.Data;
using Fody;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace Demon.Fody
{
    public class TypeWeaver
    {
        readonly TypeDefinition _type;
        readonly List<AdviceModel> _adviceModels;

        public TypeWeaver(TypeDefinition type, List<AdviceModel> adviceModels) =>
            (_type, _adviceModels) = (type, adviceModels);

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

        //todo filter async
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

        //todo support multiple public constructors use a dag and filter by :this(...) calls
        //todo compose nicely multiple aspects
        static void WeaveInstance(MethodDefinition target, MethodDefinition advice)
        {
            //todo move declaring type manipulation elsewhere
            var targetType = target.DeclaringType;
            var aspect = advice.DeclaringType;

            //todo do i need to add other attributes or customAttributes?
            var field = new FieldDefinition($"_<Demon<Aspect<{aspect.Name}", FieldAttributes.Private | FieldAttributes.InitOnly, aspect);
            targetType.Fields.Add(field);

            var constructors = targetType.GetConstructors()
                .Where(c => c.IsPublic)
                .ToList();

            if (constructors.Count != 1)
                //todo make message nice, add context
                throw new WeavingException("There must be only one public constructor.");

            var constructor = constructors[0];

            AddAspectToConstructor(constructor, aspect, field);
        }

        //todo ugly, refac
        static void AddAspectToConstructor(MethodDefinition constructor, TypeDefinition aspect, FieldDefinition field)
        {
            var parameter = new ParameterDefinition($"<Demon<Aspect<{aspect.Name}", ParameterAttributes.None, aspect);

            constructor.Parameters.Add(parameter);

            var il = constructor.Body.GetILProcessor();
            
            var ldarg0 = il.Create(OpCodes.Ldarg_0);

            var parameterIndex = constructor.Parameters.Count;

            var ldAspect = TryUseEfficientLoadInstruction(il, parameterIndex) ?? il.Create(OpCodes.Ldarg_S, parameter);

            var stfld = il.Create(OpCodes.Stfld, field);

            var originalRet = constructor.Body.Instructions.Last();
            
            il.InsertBefore(originalRet,ldarg0);
            il.InsertBefore(originalRet,ldAspect);
            il.InsertBefore(originalRet,stfld);
        }

        //todo move
        static Instruction TryUseEfficientLoadInstruction(ILProcessor il, int index)
        {
            switch (index)
            {
                case 0:
                    return il.Create(OpCodes.Ldarg_0);
                case 1:
                    return il.Create(OpCodes.Ldarg_1);
                case 2:
                    return il.Create(OpCodes.Ldarg_2);
                case 3:
                    return il.Create(OpCodes.Ldarg_3);
                default:
                    return null;
            }
        }
    }
}