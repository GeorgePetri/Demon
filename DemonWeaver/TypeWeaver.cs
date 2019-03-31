using System;
using System.Collections.Generic;
using System.Linq;
using DemonWeaver.Data;
using DemonWeaver.Extensions;
using DemonWeaver.IlEmitter;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Mono.Collections.Generic;

namespace DemonWeaver
{
    //todo add debug info
    //todo add option for the user to insert the aspect constructor parameter , for non di instantiation                           
    public class TypeWeaver
    {
        readonly TypeDefinition _type;
        readonly List<AdviceModel> _adviceModels;
        readonly DemonTypes _demonTypes;

        public TypeWeaver(TypeDefinition type, List<AdviceModel> adviceModels, DemonTypes demonTypes) =>
            (_type, _adviceModels, _demonTypes) = (type, adviceModels, demonTypes);

        public static void Weave(TypeDefinition type, List<AdviceModel> aspects, DemonTypes demonTypes) =>
            new TypeWeaver(type, aspects, demonTypes).Weave();

        void Weave()
        {
            foreach (var method in new Collection<MethodDefinition>(_type.Methods))
            {
                foreach (var advice in _adviceModels)
                {
                    if (advice.FilterToApply(method) && DefaultFilter(method))
                    {
                        switch (advice)
                        {
                            case BeforeAdvice _:
                                ApplyBeforeAdvice(method, advice.Method);
                                break;
                            case AroundAdvice _:
                                ApplyAroundAdvice(method, advice.Method);
                                break;
                            default:
                                throw new ArgumentException();
                        }
                    }
                }
            }
        }

        //todo filter async
        static bool DefaultFilter(MethodDefinition method) =>
            method.HasBody
            && method.IsPublic
            && !method.IsConstructor;

        //todo deduplicate
        void ApplyBeforeAdvice(MethodDefinition target, MethodDefinition advice)
        {
            //todo make sure importing doesn't add unneeded references or cycles that break, make sure parameter and return values work
            //todo should a dependency in the other direction work?
            var importedAdvice = target.Module.ImportReference(advice);

            if (advice.IsStatic)
                new BeforeMethodWeaver(_demonTypes, target, importedAdvice, null).Weave();
            else
            {
                var field = GetOrAddFieldIfNeeded(importedAdvice.DeclaringType);

                AddToConstructorIfNeeded(importedAdvice.DeclaringType, field);

                new BeforeMethodWeaver(_demonTypes, target, importedAdvice, field).Weave();
            }
        }

        void ApplyAroundAdvice(MethodDefinition target, MethodDefinition advice)
        {
            //todo make sure importing doesn't add unneeded references or cycles that break, make sure parameter and return values work
            //todo should a dependency in the other direction work?
            var importedAdvice = target.Module.ImportReference(advice);

            if (advice.IsStatic)
                new AroundMethodWeaver(_demonTypes, target, importedAdvice, null).Weave();
            else
            {
                var field = GetOrAddFieldIfNeeded(importedAdvice.DeclaringType);

                AddToConstructorIfNeeded(importedAdvice.DeclaringType, field);

                new AroundMethodWeaver(_demonTypes, target, importedAdvice, field).Weave();
            }
        }

        //todo compose nicely multiple aspects
        FieldDefinition GetOrAddFieldIfNeeded(TypeReference aspect)
        {
            var name = $"_<Demon<Aspect<{aspect.FullName}";

            var found = _type.Fields.FirstOrDefault(f => f.Name == name);

            if (found != null)
                return found;

            var added = new FieldDefinition(name, FieldAttributes.Private | FieldAttributes.InitOnly, aspect);
            _type.Fields.Add(added);

            return added;
        }

        //todo support multiple public constructors use a dag and filter by :this(...) calls
        void AddToConstructorIfNeeded(TypeReference aspect, FieldDefinition field)
        {
            var constructors = _type.GetConstructors()
                .Where(c => c.IsPublic)
                .ToList();

            if (constructors.Count != 1)
                throw new WeavingException($"There must be only one public constructor in type {_type.FullName}");

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

            var originalRet = constructor.Body.Instructions.Last();

            var emitter = EmitterFactory.Get(il, i => il.InsertBefore(originalRet,i));
            
            emitter.Ldarg_0();
            il.InsertBefore(originalRet,il.GetEfficientLoadInstruction(parameter) );
            emitter.Stfld(field);
        }
    }
}