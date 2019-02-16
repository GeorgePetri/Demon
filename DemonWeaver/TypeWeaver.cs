using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DemonWeaver.Data;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using FieldAttributes = Mono.Cecil.FieldAttributes;
using MethodAttributes = Mono.Cecil.MethodAttributes;
using ParameterAttributes = Mono.Cecil.ParameterAttributes;

namespace DemonWeaver
{
    //todo add debug info if needed
    //todo add option for the user to insert the aspect constructor parameter , for non di instantiation                           
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
        void WeaveStatic(MethodDefinition target, MethodDefinition advice) =>
            new MethodWeaver(this, target, advice, null).Weave();

        //todo support multiple public constructors use a dag and filter by :this(...) calls
        //todo compose nicely multiple aspects
        void WeaveInstance(MethodDefinition target, MethodDefinition advice)
        {
            var aspect = advice.DeclaringType;

            var field = GetOrAddFieldIfNeeded(aspect);

            AddToConstructorIfNeeded(aspect, field);

            new MethodWeaver(this, target, advice, field).Weave();
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

        //todo try get rid of typeJoinPointType
        //todo ugly, cleanup
        public FieldDefinition WeaveTypeJoinPointField(MethodReference method, TypeReference typeJoinPointType)
        {
            var fieldName = $"_<Demon<TypeJoinPoint<{method.FullName}";

            var field = new FieldDefinition(fieldName, FieldAttributes.Private | FieldAttributes.InitOnly | FieldAttributes.Static, typeJoinPointType);

            var staticConstructor = GetOrAddStaticConstructor();

            var il = staticConstructor.Body.GetILProcessor();

            var getMethodFromHandle = _type.Module.ImportReference(typeof(MethodBase).GetMethod(nameof(MethodBase.GetMethodFromHandle), new[] {typeof(RuntimeMethodHandle)}));
            var methodInfo = _type.Module.ImportReference(typeof(MethodInfo));

            var loadMethodToken = il.Create(OpCodes.Ldtoken, method);
            var callGetMethodFromHandle = il.Create(OpCodes.Call, getMethodFromHandle);
            var castToMethodInfo = il.Create(OpCodes.Castclass, methodInfo); //todo is this needed?

            var lastOriginalInstruction = staticConstructor.Body.Instructions.Last();
            il.InsertAfter(lastOriginalInstruction, loadMethodToken);
            il.InsertAfter(lastOriginalInstruction, callGetMethodFromHandle);
            il.InsertAfter(lastOriginalInstruction, castToMethodInfo);

            return field;
        }

        MethodDefinition GetOrAddStaticConstructor()
        {
            var currentStaticConstructor = _type.GetStaticConstructor();

            if (currentStaticConstructor != null)
                return currentStaticConstructor;

            var newStaticConstructor = new MethodDefinition(
                ".cctor",
                MethodAttributes.Private
                | MethodAttributes.Static
                | MethodAttributes.HideBySig
                | MethodAttributes.SpecialName
                | MethodAttributes.RTSpecialName,
                _type.Module.TypeSystem.Void);

            _type.Methods.Add(newStaticConstructor);

            _type.IsBeforeFieldInit = false; //todo what does this do? is it needed?

            return newStaticConstructor;
        }
    }
}