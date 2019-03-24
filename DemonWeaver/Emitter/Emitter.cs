using Mono.Cecil.Cil;

namespace DemonWeaver.Emitter
{
    public class Emitter : IEmitter<Instruction>
    {
        readonly ILProcessor _il;

        public Instruction Nop()
        {
            return _il.Create(OpCodes.Nop);
        }

        public Instruction Break()
        {
            return _il.Create(OpCodes.Break);
        }

        public Instruction Ldarg_0()
        {
            return _il.Create(OpCodes.Ldarg_0);
        }

        public Instruction Ldarg_1()
        {
            return _il.Create(OpCodes.Ldarg_1);
        }

        public Instruction Ldarg_2()
        {
            return _il.Create(OpCodes.Ldarg_2);
        }

        public Instruction Ldarg_3()
        {
            return _il.Create(OpCodes.Ldarg_3);
        }

        public Instruction Ldloc_0()
        {
            return _il.Create(OpCodes.Ldloc_0);
        }

        public Instruction Ldloc_1()
        {
            return _il.Create(OpCodes.Ldloc_1);
        }

        public Instruction Ldloc_2()
        {
            return _il.Create(OpCodes.Ldloc_2);
        }

        public Instruction Ldloc_3()
        {
            return _il.Create(OpCodes.Ldloc_3);
        }

        public Instruction Stloc_0()
        {
            return _il.Create(OpCodes.Stloc_0);
        }

        public Instruction Stloc_1()
        {
            return _il.Create(OpCodes.Stloc_1);
        }

        public Instruction Stloc_2()
        {
            return _il.Create(OpCodes.Stloc_2);
        }

        public Instruction Stloc_3()
        {
            return _il.Create(OpCodes.Stloc_3);
        }

        public Instruction Ldarg_S()
        {
            return _il.Create(OpCodes.Ldarg_S);
        }

        public Instruction Ldarga_S()
        {
            return _il.Create(OpCodes.Ldarga_S);
        }

        public Instruction Starg_S()
        {
            return _il.Create(OpCodes.Starg_S);
        }

        public Instruction Ldloc_S()
        {
            return _il.Create(OpCodes.Ldloc_S);
        }

        public Instruction Ldloca_S()
        {
            return _il.Create(OpCodes.Ldloca_S);
        }

        public Instruction Stloc_S()
        {
            return _il.Create(OpCodes.Stloc_S);
        }

        public Instruction Ldnull()
        {
            return _il.Create(OpCodes.Ldnull);
        }

        public Instruction Ldc_I4_M1()
        {
            return _il.Create(OpCodes.Ldc_I4_M1);
        }

        public Instruction Ldc_I4_0()
        {
            return _il.Create(OpCodes.Ldc_I4_0);
        }

        public Instruction Ldc_I4_1()
        {
            return _il.Create(OpCodes.Ldc_I4_1);
        }

        public Instruction Ldc_I4_2()
        {
            return _il.Create(OpCodes.Ldc_I4_2);
        }

        public Instruction Ldc_I4_3()
        {
            return _il.Create(OpCodes.Ldc_I4_3);
        }

        public Instruction Ldc_I4_4()
        {
            return _il.Create(OpCodes.Ldc_I4_4);
        }

        public Instruction Ldc_I4_5()
        {
            return _il.Create(OpCodes.Ldc_I4_5);
        }

        public Instruction Ldc_I4_6()
        {
            return _il.Create(OpCodes.Ldc_I4_6);
        }

        public Instruction Ldc_I4_7()
        {
            return _il.Create(OpCodes.Ldc_I4_7);
        }

        public Instruction Ldc_I4_8()
        {
            return _il.Create(OpCodes.Ldc_I4_8);
        }

        public Instruction Ldc_I4_S()
        {
            return _il.Create(OpCodes.Ldc_I4_S);
        }

        public Instruction Ldc_I4()
        {
            return _il.Create(OpCodes.Ldc_I4);
        }

        public Instruction Ldc_I8()
        {
            return _il.Create(OpCodes.Ldc_I8);
        }

        public Instruction Ldc_R4()
        {
            return _il.Create(OpCodes.Ldc_R4);
        }

        public Instruction Ldc_R8()
        {
            return _il.Create(OpCodes.Ldc_R8);
        }

        public Instruction Dup()
        {
            return _il.Create(OpCodes.Dup);
        }

        public Instruction Pop()
        {
            return _il.Create(OpCodes.Pop);
        }

        public Instruction Jmp()
        {
            return _il.Create(OpCodes.Jmp);
        }

        public Instruction Call()
        {
            return _il.Create(OpCodes.Call);
        }

        public Instruction Calli()
        {
            return _il.Create(OpCodes.Calli);
        }

        public Instruction Ret()
        {
            return _il.Create(OpCodes.Ret);
        }

        public Instruction Br_S()
        {
            return _il.Create(OpCodes.Br_S);
        }

        public Instruction Brfalse_S()
        {
            return _il.Create(OpCodes.Brfalse_S);
        }

        public Instruction Brtrue_S()
        {
            return _il.Create(OpCodes.Brtrue_S);
        }

        public Instruction Beq_S()
        {
            return _il.Create(OpCodes.Beq_S);
        }

        public Instruction Bge_S()
        {
            return _il.Create(OpCodes.Bge_S);
        }

        public Instruction Bgt_S()
        {
            return _il.Create(OpCodes.Bgt_S);
        }

        public Instruction Ble_S()
        {
            return _il.Create(OpCodes.Ble_S);
        }

        public Instruction Blt_S()
        {
            return _il.Create(OpCodes.Blt_S);
        }

        public Instruction Bne_Un_S()
        {
            return _il.Create(OpCodes.Bne_Un_S);
        }

        public Instruction Bge_Un_S()
        {
            return _il.Create(OpCodes.Bge_Un_S);
        }

        public Instruction Bgt_Un_S()
        {
            return _il.Create(OpCodes.Bgt_Un_S);
        }

        public Instruction Ble_Un_S()
        {
            return _il.Create(OpCodes.Ble_Un_S);
        }

        public Instruction Blt_Un_S()
        {
            return _il.Create(OpCodes.Blt_Un_S);
        }

        public Instruction Br()
        {
            return _il.Create(OpCodes.Br);
        }

        public Instruction Brfalse()
        {
            return _il.Create(OpCodes.Brfalse);
        }

        public Instruction Brtrue()
        {
            return _il.Create(OpCodes.Brtrue);
        }

        public Instruction Beq()
        {
            return _il.Create(OpCodes.Beq);
        }

        public Instruction Bge()
        {
            return _il.Create(OpCodes.Bge);
        }

        public Instruction Bgt()
        {
            return _il.Create(OpCodes.Bgt);
        }

        public Instruction Ble()
        {
            return _il.Create(OpCodes.Ble);
        }

        public Instruction Blt()
        {
            return _il.Create(OpCodes.Blt);
        }

        public Instruction Bne_Un()
        {
            return _il.Create(OpCodes.Bne_Un);
        }

        public Instruction Bge_Un()
        {
            return _il.Create(OpCodes.Bge_Un);
        }

        public Instruction Bgt_Un()
        {
            return _il.Create(OpCodes.Bgt_Un);
        }

        public Instruction Ble_Un()
        {
            return _il.Create(OpCodes.Ble_Un);
        }

        public Instruction Blt_Un()
        {
            return _il.Create(OpCodes.Blt_Un);
        }

        public Instruction Switch()
        {
            return _il.Create(OpCodes.Switch);
        }

        public Instruction Ldind_I1()
        {
            return _il.Create(OpCodes.Ldind_I1);
        }

        public Instruction Ldind_U1()
        {
            return _il.Create(OpCodes.Ldind_U1);
        }

        public Instruction Ldind_I2()
        {
            return _il.Create(OpCodes.Ldind_I2);
        }

        public Instruction Ldind_U2()
        {
            return _il.Create(OpCodes.Ldind_U2);
        }

        public Instruction Ldind_I4()
        {
            return _il.Create(OpCodes.Ldind_I4);
        }

        public Instruction Ldind_U4()
        {
            return _il.Create(OpCodes.Ldind_U4);
        }

        public Instruction Ldind_I8()
        {
            return _il.Create(OpCodes.Ldind_I8);
        }

        public Instruction Ldind_I()
        {
            return _il.Create(OpCodes.Ldind_I);
        }

        public Instruction Ldind_R4()
        {
            return _il.Create(OpCodes.Ldind_R4);
        }

        public Instruction Ldind_R8()
        {
            return _il.Create(OpCodes.Ldind_R8);
        }

        public Instruction Ldind_Ref()
        {
            return _il.Create(OpCodes.Ldind_Ref);
        }

        public Instruction Stind_Ref()
        {
            return _il.Create(OpCodes.Stind_Ref);
        }

        public Instruction Stind_I1()
        {
            return _il.Create(OpCodes.Stind_I1);
        }

        public Instruction Stind_I2()
        {
            return _il.Create(OpCodes.Stind_I2);
        }

        public Instruction Stind_I4()
        {
            return _il.Create(OpCodes.Stind_I4);
        }

        public Instruction Stind_I8()
        {
            return _il.Create(OpCodes.Stind_I8);
        }

        public Instruction Stind_R4()
        {
            return _il.Create(OpCodes.Stind_R4);
        }

        public Instruction Stind_R8()
        {
            return _il.Create(OpCodes.Stind_R8);
        }

        public Instruction Add()
        {
            return _il.Create(OpCodes.Add);
        }

        public Instruction Sub()
        {
            return _il.Create(OpCodes.Sub);
        }

        public Instruction Mul()
        {
            return _il.Create(OpCodes.Mul);
        }

        public Instruction Div()
        {
            return _il.Create(OpCodes.Div);
        }

        public Instruction Div_Un()
        {
            return _il.Create(OpCodes.Div_Un);
        }

        public Instruction Rem()
        {
            return _il.Create(OpCodes.Rem);
        }

        public Instruction Rem_Un()
        {
            return _il.Create(OpCodes.Rem_Un);
        }

        public Instruction And()
        {
            return _il.Create(OpCodes.And);
        }

        public Instruction Or()
        {
            return _il.Create(OpCodes.Or);
        }

        public Instruction Xor()
        {
            return _il.Create(OpCodes.Xor);
        }

        public Instruction Shl()
        {
            return _il.Create(OpCodes.Shl);
        }

        public Instruction Shr()
        {
            return _il.Create(OpCodes.Shr);
        }

        public Instruction Shr_Un()
        {
            return _il.Create(OpCodes.Shr_Un);
        }

        public Instruction Neg()
        {
            return _il.Create(OpCodes.Neg);
        }

        public Instruction Not()
        {
            return _il.Create(OpCodes.Not);
        }

        public Instruction Conv_I1()
        {
            return _il.Create(OpCodes.Conv_I1);
        }

        public Instruction Conv_I2()
        {
            return _il.Create(OpCodes.Conv_I2);
        }

        public Instruction Conv_I4()
        {
            return _il.Create(OpCodes.Conv_I4);
        }

        public Instruction Conv_I8()
        {
            return _il.Create(OpCodes.Conv_I8);
        }

        public Instruction Conv_R4()
        {
            return _il.Create(OpCodes.Conv_R4);
        }

        public Instruction Conv_R8()
        {
            return _il.Create(OpCodes.Conv_R8);
        }

        public Instruction Conv_U4()
        {
            return _il.Create(OpCodes.Conv_U4);
        }

        public Instruction Conv_U8()
        {
            return _il.Create(OpCodes.Conv_U8);
        }

        public Instruction Callvirt()
        {
            return _il.Create(OpCodes.Callvirt);
        }

        public Instruction Cpobj()
        {
            return _il.Create(OpCodes.Cpobj);
        }

        public Instruction Ldobj()
        {
            return _il.Create(OpCodes.Ldobj);
        }

        public Instruction Ldstr()
        {
            return _il.Create(OpCodes.Ldstr);
        }

        public Instruction Newobj()
        {
            return _il.Create(OpCodes.Newobj);
        }

        public Instruction Castclass()
        {
            return _il.Create(OpCodes.Castclass);
        }

        public Instruction Isinst()
        {
            return _il.Create(OpCodes.Isinst);
        }

        public Instruction Conv_R_Un()
        {
            return _il.Create(OpCodes.Conv_R_Un);
        }

        public Instruction Unbox()
        {
            return _il.Create(OpCodes.Unbox);
        }

        public Instruction Throw()
        {
            return _il.Create(OpCodes.Throw);
        }

        public Instruction Ldfld()
        {
            return _il.Create(OpCodes.Ldfld);
        }

        public Instruction Ldflda()
        {
            return _il.Create(OpCodes.Ldflda);
        }

        public Instruction Stfld()
        {
            return _il.Create(OpCodes.Stfld);
        }

        public Instruction Ldsfld()
        {
            return _il.Create(OpCodes.Ldsfld);
        }

        public Instruction Ldsflda()
        {
            return _il.Create(OpCodes.Ldsflda);
        }

        public Instruction Stsfld()
        {
            return _il.Create(OpCodes.Stsfld);
        }

        public Instruction Stobj()
        {
            return _il.Create(OpCodes.Stobj);
        }

        public Instruction Conv_Ovf_I1_Un()
        {
            return _il.Create(OpCodes.Conv_Ovf_I1_Un);
        }

        public Instruction Conv_Ovf_I2_Un()
        {
            return _il.Create(OpCodes.Conv_Ovf_I2_Un);
        }

        public Instruction Conv_Ovf_I4_Un()
        {
            return _il.Create(OpCodes.Conv_Ovf_I4_Un);
        }

        public Instruction Conv_Ovf_I8_Un()
        {
            return _il.Create(OpCodes.Conv_Ovf_I8_Un);
        }

        public Instruction Conv_Ovf_U1_Un()
        {
            return _il.Create(OpCodes.Conv_Ovf_U1_Un);
        }

        public Instruction Conv_Ovf_U2_Un()
        {
            return _il.Create(OpCodes.Conv_Ovf_U2_Un);
        }

        public Instruction Conv_Ovf_U4_Un()
        {
            return _il.Create(OpCodes.Conv_Ovf_U4_Un);
        }

        public Instruction Conv_Ovf_U8_Un()
        {
            return _il.Create(OpCodes.Conv_Ovf_U8_Un);
        }

        public Instruction Conv_Ovf_I_Un()
        {
            return _il.Create(OpCodes.Conv_Ovf_I_Un);
        }

        public Instruction Conv_Ovf_U_Un()
        {
            return _il.Create(OpCodes.Conv_Ovf_U_Un);
        }

        public Instruction Box()
        {
            return _il.Create(OpCodes.Box);
        }

        public Instruction Newarr()
        {
            return _il.Create(OpCodes.Newarr);
        }

        public Instruction Ldlen()
        {
            return _il.Create(OpCodes.Ldlen);
        }

        public Instruction Ldelema()
        {
            return _il.Create(OpCodes.Ldelema);
        }

        public Instruction Ldelem_I1()
        {
            return _il.Create(OpCodes.Ldelem_I1);
        }

        public Instruction Ldelem_U1()
        {
            return _il.Create(OpCodes.Ldelem_U1);
        }

        public Instruction Ldelem_I2()
        {
            return _il.Create(OpCodes.Ldelem_I2);
        }

        public Instruction Ldelem_U2()
        {
            return _il.Create(OpCodes.Ldelem_U2);
        }

        public Instruction Ldelem_I4()
        {
            return _il.Create(OpCodes.Ldelem_I4);
        }

        public Instruction Ldelem_U4()
        {
            return _il.Create(OpCodes.Ldelem_U4);
        }

        public Instruction Ldelem_I8()
        {
            return _il.Create(OpCodes.Ldelem_I8);
        }

        public Instruction Ldelem_I()
        {
            return _il.Create(OpCodes.Ldelem_I);
        }

        public Instruction Ldelem_R4()
        {
            return _il.Create(OpCodes.Ldelem_R4);
        }

        public Instruction Ldelem_R8()
        {
            return _il.Create(OpCodes.Ldelem_R8);
        }

        public Instruction Ldelem_Ref()
        {
            return _il.Create(OpCodes.Ldelem_Ref);
        }

        public Instruction Stelem_I()
        {
            return _il.Create(OpCodes.Stelem_I);
        }

        public Instruction Stelem_I1()
        {
            return _il.Create(OpCodes.Stelem_I1);
        }

        public Instruction Stelem_I2()
        {
            return _il.Create(OpCodes.Stelem_I2);
        }

        public Instruction Stelem_I4()
        {
            return _il.Create(OpCodes.Stelem_I4);
        }

        public Instruction Stelem_I8()
        {
            return _il.Create(OpCodes.Stelem_I8);
        }

        public Instruction Stelem_R4()
        {
            return _il.Create(OpCodes.Stelem_R4);
        }

        public Instruction Stelem_R8()
        {
            return _il.Create(OpCodes.Stelem_R8);
        }

        public Instruction Stelem_Ref()
        {
            return _il.Create(OpCodes.Stelem_Ref);
        }

        public Instruction Ldelem_Any()
        {
            return _il.Create(OpCodes.Ldelem_Any);
        }

        public Instruction Stelem_Any()
        {
            return _il.Create(OpCodes.Stelem_Any);
        }

        public Instruction Unbox_Any()
        {
            return _il.Create(OpCodes.Unbox_Any);
        }

        public Instruction Conv_Ovf_I1()
        {
            return _il.Create(OpCodes.Conv_Ovf_I1);
        }

        public Instruction Conv_Ovf_U1()
        {
            return _il.Create(OpCodes.Conv_Ovf_U1);
        }

        public Instruction Conv_Ovf_I2()
        {
            return _il.Create(OpCodes.Conv_Ovf_I2);
        }

        public Instruction Conv_Ovf_U2()
        {
            return _il.Create(OpCodes.Conv_Ovf_U2);
        }

        public Instruction Conv_Ovf_I4()
        {
            return _il.Create(OpCodes.Conv_Ovf_I4);
        }

        public Instruction Conv_Ovf_U4()
        {
            return _il.Create(OpCodes.Conv_Ovf_U4);
        }

        public Instruction Conv_Ovf_I8()
        {
            return _il.Create(OpCodes.Conv_Ovf_I8);
        }

        public Instruction Conv_Ovf_U8()
        {
            return _il.Create(OpCodes.Conv_Ovf_U8);
        }

        public Instruction Refanyval()
        {
            return _il.Create(OpCodes.Refanyval);
        }

        public Instruction Ckfinite()
        {
            return _il.Create(OpCodes.Ckfinite);
        }

        public Instruction Mkrefany()
        {
            return _il.Create(OpCodes.Mkrefany);
        }

        public Instruction Ldtoken()
        {
            return _il.Create(OpCodes.Ldtoken);
        }

        public Instruction Conv_U2()
        {
            return _il.Create(OpCodes.Conv_U2);
        }

        public Instruction Conv_U1()
        {
            return _il.Create(OpCodes.Conv_U1);
        }

        public Instruction Conv_I()
        {
            return _il.Create(OpCodes.Conv_I);
        }

        public Instruction Conv_Ovf_I()
        {
            return _il.Create(OpCodes.Conv_Ovf_I);
        }

        public Instruction Conv_Ovf_U()
        {
            return _il.Create(OpCodes.Conv_Ovf_U);
        }

        public Instruction Add_Ovf()
        {
            return _il.Create(OpCodes.Add_Ovf);
        }

        public Instruction Add_Ovf_Un()
        {
            return _il.Create(OpCodes.Add_Ovf_Un);
        }

        public Instruction Mul_Ovf()
        {
            return _il.Create(OpCodes.Mul_Ovf);
        }

        public Instruction Mul_Ovf_Un()
        {
            return _il.Create(OpCodes.Mul_Ovf_Un);
        }

        public Instruction Sub_Ovf()
        {
            return _il.Create(OpCodes.Sub_Ovf);
        }

        public Instruction Sub_Ovf_Un()
        {
            return _il.Create(OpCodes.Sub_Ovf_Un);
        }

        public Instruction Endfinally()
        {
            return _il.Create(OpCodes.Endfinally);
        }

        public Instruction Leave()
        {
            return _il.Create(OpCodes.Leave);
        }

        public Instruction Leave_S()
        {
            return _il.Create(OpCodes.Leave_S);
        }

        public Instruction Stind_I()
        {
            return _il.Create(OpCodes.Stind_I);
        }

        public Instruction Conv_U()
        {
            return _il.Create(OpCodes.Conv_U);
        }

        public Instruction Arglist()
        {
            return _il.Create(OpCodes.Arglist);
        }

        public Instruction Ceq()
        {
            return _il.Create(OpCodes.Ceq);
        }

        public Instruction Cgt()
        {
            return _il.Create(OpCodes.Cgt);
        }

        public Instruction Cgt_Un()
        {
            return _il.Create(OpCodes.Cgt_Un);
        }

        public Instruction Clt()
        {
            return _il.Create(OpCodes.Clt);
        }

        public Instruction Clt_Un()
        {
            return _il.Create(OpCodes.Clt_Un);
        }

        public Instruction Ldftn()
        {
            return _il.Create(OpCodes.Ldftn);
        }

        public Instruction Ldvirtftn()
        {
            return _il.Create(OpCodes.Ldvirtftn);
        }

        public Instruction Ldarg()
        {
            return _il.Create(OpCodes.Ldarg);
        }

        public Instruction Ldarga()
        {
            return _il.Create(OpCodes.Ldarga);
        }

        public Instruction Starg()
        {
            return _il.Create(OpCodes.Starg);
        }

        public Instruction Ldloc()
        {
            return _il.Create(OpCodes.Ldloc);
        }

        public Instruction Ldloca()
        {
            return _il.Create(OpCodes.Ldloca);
        }

        public Instruction Stloc()
        {
            return _il.Create(OpCodes.Stloc);
        }

        public Instruction Localloc()
        {
            return _il.Create(OpCodes.Localloc);
        }

        public Instruction Endfilter()
        {
            return _il.Create(OpCodes.Endfilter);
        }

        public Instruction Unaligned()
        {
            return _il.Create(OpCodes.Unaligned);
        }

        public Instruction Volatile()
        {
            return _il.Create(OpCodes.Volatile);
        }

        public Instruction Tail()
        {
            return _il.Create(OpCodes.Tail);
        }

        public Instruction Initobj()
        {
            return _il.Create(OpCodes.Initobj);
        }

        public Instruction Constrained()
        {
            return _il.Create(OpCodes.Constrained);
        }

        public Instruction Cpblk()
        {
            return _il.Create(OpCodes.Cpblk);
        }

        public Instruction Initblk()
        {
            return _il.Create(OpCodes.Initblk);
        }

        public Instruction No()
        {
            return _il.Create(OpCodes.No);
        }

        public Instruction Rethrow()
        {
            return _il.Create(OpCodes.Rethrow);
        }

        public Instruction Sizeof()
        {
            return _il.Create(OpCodes.Sizeof);
        }

        public Instruction Refanytype()
        {
            return _il.Create(OpCodes.Refanytype);
        }

        public Instruction Readonly()
        {
            return _il.Create(OpCodes.Readonly);
        }
    }
}