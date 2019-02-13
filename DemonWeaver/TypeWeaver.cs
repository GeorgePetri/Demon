using System.Collections.Generic;
using System.Linq;
using DemonWeaver.Data;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace DemonWeaver
{
    //todo add debug info if needed
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
        void ApplyAdvice(MethodDefinition method, AdviceModel advice)
        {
            if (advice.Method.IsStatic)
                WeaveStatic(method, advice.Method);
            else
                WeaveInstance(method, advice.Method);
        }

        //todo combine this and instance
        static void WeaveStatic(MethodDefinition target, MethodDefinition advice) =>
            new MethodWeaver(target, advice, null).Weave();

        //todo support multiple public constructors use a dag and filter by :this(...) calls
        //todo compose nicely multiple aspects
        void WeaveInstance(MethodDefinition target, MethodDefinition advice)
        {
            var aspect = advice.DeclaringType;

            var field = GetOrAddFieldIfNeeded(aspect);

            AddToConstructorIfNeeded(aspect, field);

            new MethodWeaver(target, advice, field).Weave();
        }

        FieldDefinition GetOrAddFieldIfNeeded(TypeDefinition aspect)
        {
            var name = $"_<Demon<Aspect<{aspect.FullName}";

            var found = _type.Fields.FirstOrDefault(f => f.Name == name);

            if (found != null)
                return found;

            var added = new FieldDefinition(name, FieldAttributes.Private | FieldAttributes.InitOnly, aspect);
            _type.Fields.Add(added);

            return added;
        }

        void AddToConstructorIfNeeded(TypeDefinition aspect, FieldDefinition field)
        {
            var constructors = _type.GetConstructors()
                .Where(c => c.IsPublic)
                .ToList();

            if (constructors.Count != 1)
                //todo make message nice, add context
                throw new WeavingException("There must be only one public constructor.");

            var constructor = constructors[0];

            var parameterName = $"<Demon<Aspect<{aspect.FullName}";

            if (constructor.Parameters.Any(p => p.Name == parameterName))
                return;

            var parameter = new ParameterDefinition(parameterName, ParameterAttributes.None, aspect);

            AddAspectToConstructor(constructor, parameter, field);
        }

        //todo ugly, refac
        static void AddAspectToConstructor(MethodDefinition constructor, ParameterDefinition parameter, FieldDefinition field)
        {
            constructor.Parameters.Add(parameter);

            var il = constructor.Body.GetILProcessor();

            var ldarg0 = il.Create(OpCodes.Ldarg_0);

            var ldAspect = il.GetEfficientLoadInstruction(parameter);

            var stfld = il.Create(OpCodes.Stfld, field);

            var originalRet = constructor.Body.Instructions.Last();

            il.InsertBefore(originalRet, ldarg0);
            il.InsertBefore(originalRet, ldAspect);
            il.InsertBefore(originalRet, stfld);
        }
    }
}