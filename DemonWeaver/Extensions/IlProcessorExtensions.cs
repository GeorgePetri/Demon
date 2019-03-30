using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace DemonWeaver.Extensions
{
    //todo add higher level emitter like class for these
    public static class IlProcessorExtensions
    {
        public static Instruction GetEfficientLoadInstruction(this ILProcessor il, ParameterDefinition parameterDefinition)
        {
            switch (parameterDefinition.Sequence)
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
                    return il.Create(parameterDefinition.Sequence < 255 ? OpCodes.Ldarg_S : OpCodes.Ldarg, parameterDefinition);
            }
        }

        //todo doesn't handle every case
        public static Instruction GetLoadForDefault(this ILProcessor il, TypeReference type)
        {
            if (!type.IsValueType)
                return il.Create(OpCodes.Ldnull);

            if (type.FullName == "System.Int32")
                return il.Create(OpCodes.Ldc_I4_0);

            throw new NotImplementedException($"No default value for type {type.FullName} found.");
        }
    }
}