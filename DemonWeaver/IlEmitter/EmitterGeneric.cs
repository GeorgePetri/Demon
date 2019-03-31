using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace DemonWeaver.IlEmitter
{
    public class Emitter<TReturn>
    {
        readonly ILProcessor _il;
        readonly Func<Instruction, TReturn> _func;

        public Emitter(ILProcessor il, Func<Instruction, TReturn> func) => (_il, _func) = (il, func);

        public TReturn Nop() =>
            _func(_il.Create(OpCodes.Nop));

        public TReturn Break() =>
            _func(_il.Create(OpCodes.Break));

        public TReturn Ldarg_0() =>
            _func(_il.Create(OpCodes.Ldarg_0));

        public TReturn Ldarg_1() =>
            _func(_il.Create(OpCodes.Ldarg_1));

        public TReturn Ldarg_2() =>
            _func(_il.Create(OpCodes.Ldarg_2));

        public TReturn Ldarg_3() =>
            _func(_il.Create(OpCodes.Ldarg_3));

        public TReturn Ldloc_0() =>
            _func(_il.Create(OpCodes.Ldloc_0));

        public TReturn Ldloc_1() =>
            _func(_il.Create(OpCodes.Ldloc_1));

        public TReturn Ldloc_2() =>
            _func(_il.Create(OpCodes.Ldloc_2));

        public TReturn Ldloc_3() =>
            _func(_il.Create(OpCodes.Ldloc_3));

        public TReturn Stloc_0() =>
            _func(_il.Create(OpCodes.Stloc_0));

        public TReturn Stloc_1() =>
            _func(_il.Create(OpCodes.Stloc_1));

        public TReturn Stloc_2() =>
            _func(_il.Create(OpCodes.Stloc_2));

        public TReturn Stloc_3() =>
            _func(_il.Create(OpCodes.Stloc_3));

        public TReturn Ldarg_S() =>
            _func(_il.Create(OpCodes.Ldarg_S));

        public TReturn Ldarga_S() =>
            _func(_il.Create(OpCodes.Ldarga_S));

        public TReturn Starg_S() =>
            _func(_il.Create(OpCodes.Starg_S));

        public TReturn Ldloc_S() =>
            _func(_il.Create(OpCodes.Ldloc_S));

        public TReturn Ldloca_S() =>
            _func(_il.Create(OpCodes.Ldloca_S));

        public TReturn Stloc_S() =>
            _func(_il.Create(OpCodes.Stloc_S));

        public TReturn Ldnull() =>
            _func(_il.Create(OpCodes.Ldnull));

        public TReturn Ldc_I4_M1() =>
            _func(_il.Create(OpCodes.Ldc_I4_M1));

        public TReturn Ldc_I4_0() =>
            _func(_il.Create(OpCodes.Ldc_I4_0));

        public TReturn Ldc_I4_1() =>
            _func(_il.Create(OpCodes.Ldc_I4_1));

        public TReturn Ldc_I4_2() =>
            _func(_il.Create(OpCodes.Ldc_I4_2));

        public TReturn Ldc_I4_3() =>
            _func(_il.Create(OpCodes.Ldc_I4_3));

        public TReturn Ldc_I4_4() =>
            _func(_il.Create(OpCodes.Ldc_I4_4));

        public TReturn Ldc_I4_5() =>
            _func(_il.Create(OpCodes.Ldc_I4_5));

        public TReturn Ldc_I4_6() =>
            _func(_il.Create(OpCodes.Ldc_I4_6));

        public TReturn Ldc_I4_7() =>
            _func(_il.Create(OpCodes.Ldc_I4_7));

        public TReturn Ldc_I4_8() =>
            _func(_il.Create(OpCodes.Ldc_I4_8));

        public TReturn Ldc_I4_S() =>
            _func(_il.Create(OpCodes.Ldc_I4_S));

        public TReturn Ldc_I4() =>
            _func(_il.Create(OpCodes.Ldc_I4));

        public TReturn Ldc_I8() =>
            _func(_il.Create(OpCodes.Ldc_I8));

        public TReturn Ldc_R4() =>
            _func(_il.Create(OpCodes.Ldc_R4));

        public TReturn Ldc_R8() =>
            _func(_il.Create(OpCodes.Ldc_R8));

        public TReturn Dup() =>
            _func(_il.Create(OpCodes.Dup));

        public TReturn Pop() =>
            _func(_il.Create(OpCodes.Pop));

        public TReturn Jmp() =>
            _func(_il.Create(OpCodes.Jmp));

        public TReturn Call(MethodReference method) =>
            _func(_il.Create(OpCodes.Call, method));

        public TReturn Calli() =>
            _func(_il.Create(OpCodes.Calli));

        public TReturn Ret() =>
            _func(_il.Create(OpCodes.Ret));

        public TReturn Br_S() =>
            _func(_il.Create(OpCodes.Br_S));

        public TReturn Brfalse_S() =>
            _func(_il.Create(OpCodes.Brfalse_S));

        public TReturn Brtrue_S() =>
            _func(_il.Create(OpCodes.Brtrue_S));

        public TReturn Beq_S() =>
            _func(_il.Create(OpCodes.Beq_S));

        public TReturn Bge_S() =>
            _func(_il.Create(OpCodes.Bge_S));

        public TReturn Bgt_S() =>
            _func(_il.Create(OpCodes.Bgt_S));

        public TReturn Ble_S() =>
            _func(_il.Create(OpCodes.Ble_S));

        public TReturn Blt_S() =>
            _func(_il.Create(OpCodes.Blt_S));

        public TReturn Bne_Un_S() =>
            _func(_il.Create(OpCodes.Bne_Un_S));

        public TReturn Bge_Un_S() =>
            _func(_il.Create(OpCodes.Bge_Un_S));

        public TReturn Bgt_Un_S() =>
            _func(_il.Create(OpCodes.Bgt_Un_S));

        public TReturn Ble_Un_S() =>
            _func(_il.Create(OpCodes.Ble_Un_S));

        public TReturn Blt_Un_S() =>
            _func(_il.Create(OpCodes.Blt_Un_S));

        public TReturn Br() =>
            _func(_il.Create(OpCodes.Br));

        public TReturn Brfalse() =>
            _func(_il.Create(OpCodes.Brfalse));

        public TReturn Brtrue() =>
            _func(_il.Create(OpCodes.Brtrue));

        public TReturn Beq() =>
            _func(_il.Create(OpCodes.Beq));

        public TReturn Bge() =>
            _func(_il.Create(OpCodes.Bge));

        public TReturn Bgt() =>
            _func(_il.Create(OpCodes.Bgt));

        public TReturn Ble() =>
            _func(_il.Create(OpCodes.Ble));

        public TReturn Blt() =>
            _func(_il.Create(OpCodes.Blt));

        public TReturn Bne_Un() =>
            _func(_il.Create(OpCodes.Bne_Un));

        public TReturn Bge_Un() =>
            _func(_il.Create(OpCodes.Bge_Un));

        public TReturn Bgt_Un() =>
            _func(_il.Create(OpCodes.Bgt_Un));

        public TReturn Ble_Un() =>
            _func(_il.Create(OpCodes.Ble_Un));

        public TReturn Blt_Un() =>
            _func(_il.Create(OpCodes.Blt_Un));

        public TReturn Switch() =>
            _func(_il.Create(OpCodes.Switch));

        public TReturn Ldind_I1() =>
            _func(_il.Create(OpCodes.Ldind_I1));

        public TReturn Ldind_U1() =>
            _func(_il.Create(OpCodes.Ldind_U1));

        public TReturn Ldind_I2() =>
            _func(_il.Create(OpCodes.Ldind_I2));

        public TReturn Ldind_U2() =>
            _func(_il.Create(OpCodes.Ldind_U2));

        public TReturn Ldind_I4() =>
            _func(_il.Create(OpCodes.Ldind_I4));

        public TReturn Ldind_U4() =>
            _func(_il.Create(OpCodes.Ldind_U4));

        public TReturn Ldind_I8() =>
            _func(_il.Create(OpCodes.Ldind_I8));

        public TReturn Ldind_I() =>
            _func(_il.Create(OpCodes.Ldind_I));

        public TReturn Ldind_R4() =>
            _func(_il.Create(OpCodes.Ldind_R4));

        public TReturn Ldind_R8() =>
            _func(_il.Create(OpCodes.Ldind_R8));

        public TReturn Ldind_Ref() =>
            _func(_il.Create(OpCodes.Ldind_Ref));

        public TReturn Stind_Ref() =>
            _func(_il.Create(OpCodes.Stind_Ref));

        public TReturn Stind_I1() =>
            _func(_il.Create(OpCodes.Stind_I1));

        public TReturn Stind_I2() =>
            _func(_il.Create(OpCodes.Stind_I2));

        public TReturn Stind_I4() =>
            _func(_il.Create(OpCodes.Stind_I4));

        public TReturn Stind_I8() =>
            _func(_il.Create(OpCodes.Stind_I8));

        public TReturn Stind_R4() =>
            _func(_il.Create(OpCodes.Stind_R4));

        public TReturn Stind_R8() =>
            _func(_il.Create(OpCodes.Stind_R8));

        public TReturn Add() =>
            _func(_il.Create(OpCodes.Add));

        public TReturn Sub() =>
            _func(_il.Create(OpCodes.Sub));

        public TReturn Mul() =>
            _func(_il.Create(OpCodes.Mul));

        public TReturn Div() =>
            _func(_il.Create(OpCodes.Div));

        public TReturn Div_Un() =>
            _func(_il.Create(OpCodes.Div_Un));

        public TReturn Rem() =>
            _func(_il.Create(OpCodes.Rem));

        public TReturn Rem_Un() =>
            _func(_il.Create(OpCodes.Rem_Un));

        public TReturn And() =>
            _func(_il.Create(OpCodes.And));

        public TReturn Or() =>
            _func(_il.Create(OpCodes.Or));

        public TReturn Xor() =>
            _func(_il.Create(OpCodes.Xor));

        public TReturn Shl() =>
            _func(_il.Create(OpCodes.Shl));

        public TReturn Shr() =>
            _func(_il.Create(OpCodes.Shr));

        public TReturn Shr_Un() =>
            _func(_il.Create(OpCodes.Shr_Un));

        public TReturn Neg() =>
            _func(_il.Create(OpCodes.Neg));

        public TReturn Not() =>
            _func(_il.Create(OpCodes.Not));

        public TReturn Conv_I1() =>
            _func(_il.Create(OpCodes.Conv_I1));

        public TReturn Conv_I2() =>
            _func(_il.Create(OpCodes.Conv_I2));

        public TReturn Conv_I4() =>
            _func(_il.Create(OpCodes.Conv_I4));

        public TReturn Conv_I8() =>
            _func(_il.Create(OpCodes.Conv_I8));

        public TReturn Conv_R4() =>
            _func(_il.Create(OpCodes.Conv_R4));

        public TReturn Conv_R8() =>
            _func(_il.Create(OpCodes.Conv_R8));

        public TReturn Conv_U4() =>
            _func(_il.Create(OpCodes.Conv_U4));

        public TReturn Conv_U8() =>
            _func(_il.Create(OpCodes.Conv_U8));

        public TReturn Callvirt() =>
            _func(_il.Create(OpCodes.Callvirt));

        public TReturn Cpobj() =>
            _func(_il.Create(OpCodes.Cpobj));

        public TReturn Ldobj() =>
            _func(_il.Create(OpCodes.Ldobj));

        public TReturn Ldstr(string value) =>
            _func(_il.Create(OpCodes.Ldstr, value));

        public TReturn Newobj(MethodReference method) =>
            _func(_il.Create(OpCodes.Newobj, method));

        public TReturn Castclass() =>
            _func(_il.Create(OpCodes.Castclass));

        public TReturn Isinst() =>
            _func(_il.Create(OpCodes.Isinst));

        public TReturn Conv_R_Un() =>
            _func(_il.Create(OpCodes.Conv_R_Un));

        public TReturn Unbox() =>
            _func(_il.Create(OpCodes.Unbox));

        public TReturn Throw() =>
            _func(_il.Create(OpCodes.Throw));

        public TReturn Ldfld(FieldReference field) =>
            _func(_il.Create(OpCodes.Ldfld, field));

        public TReturn Ldflda() =>
            _func(_il.Create(OpCodes.Ldflda));

        public TReturn Stfld() =>
            _func(_il.Create(OpCodes.Stfld));

        public TReturn Ldsfld() =>
            _func(_il.Create(OpCodes.Ldsfld));

        public TReturn Ldsflda() =>
            _func(_il.Create(OpCodes.Ldsflda));

        public TReturn Stsfld(FieldReference field) =>
            _func(_il.Create(OpCodes.Stsfld, field));

        public TReturn Stobj() =>
            _func(_il.Create(OpCodes.Stobj));

        public TReturn Conv_Ovf_I1_Un() =>
            _func(_il.Create(OpCodes.Conv_Ovf_I1_Un));

        public TReturn Conv_Ovf_I2_Un() =>
            _func(_il.Create(OpCodes.Conv_Ovf_I2_Un));

        public TReturn Conv_Ovf_I4_Un() =>
            _func(_il.Create(OpCodes.Conv_Ovf_I4_Un));

        public TReturn Conv_Ovf_I8_Un() =>
            _func(_il.Create(OpCodes.Conv_Ovf_I8_Un));

        public TReturn Conv_Ovf_U1_Un() =>
            _func(_il.Create(OpCodes.Conv_Ovf_U1_Un));

        public TReturn Conv_Ovf_U2_Un() =>
            _func(_il.Create(OpCodes.Conv_Ovf_U2_Un));

        public TReturn Conv_Ovf_U4_Un() =>
            _func(_il.Create(OpCodes.Conv_Ovf_U4_Un));

        public TReturn Conv_Ovf_U8_Un() =>
            _func(_il.Create(OpCodes.Conv_Ovf_U8_Un));

        public TReturn Conv_Ovf_I_Un() =>
            _func(_il.Create(OpCodes.Conv_Ovf_I_Un));

        public TReturn Conv_Ovf_U_Un() =>
            _func(_il.Create(OpCodes.Conv_Ovf_U_Un));

        public TReturn Box() =>
            _func(_il.Create(OpCodes.Box));

        public TReturn Newarr() =>
            _func(_il.Create(OpCodes.Newarr));

        public TReturn Ldlen() =>
            _func(_il.Create(OpCodes.Ldlen));

        public TReturn Ldelema() =>
            _func(_il.Create(OpCodes.Ldelema));

        public TReturn Ldelem_I1() =>
            _func(_il.Create(OpCodes.Ldelem_I1));

        public TReturn Ldelem_U1() =>
            _func(_il.Create(OpCodes.Ldelem_U1));

        public TReturn Ldelem_I2() =>
            _func(_il.Create(OpCodes.Ldelem_I2));

        public TReturn Ldelem_U2() =>
            _func(_il.Create(OpCodes.Ldelem_U2));

        public TReturn Ldelem_I4() =>
            _func(_il.Create(OpCodes.Ldelem_I4));

        public TReturn Ldelem_U4() =>
            _func(_il.Create(OpCodes.Ldelem_U4));

        public TReturn Ldelem_I8() =>
            _func(_il.Create(OpCodes.Ldelem_I8));

        public TReturn Ldelem_I() =>
            _func(_il.Create(OpCodes.Ldelem_I));

        public TReturn Ldelem_R4() =>
            _func(_il.Create(OpCodes.Ldelem_R4));

        public TReturn Ldelem_R8() =>
            _func(_il.Create(OpCodes.Ldelem_R8));

        public TReturn Ldelem_Ref() =>
            _func(_il.Create(OpCodes.Ldelem_Ref));

        public TReturn Stelem_I() =>
            _func(_il.Create(OpCodes.Stelem_I));

        public TReturn Stelem_I1() =>
            _func(_il.Create(OpCodes.Stelem_I1));

        public TReturn Stelem_I2() =>
            _func(_il.Create(OpCodes.Stelem_I2));

        public TReturn Stelem_I4() =>
            _func(_il.Create(OpCodes.Stelem_I4));

        public TReturn Stelem_I8() =>
            _func(_il.Create(OpCodes.Stelem_I8));

        public TReturn Stelem_R4() =>
            _func(_il.Create(OpCodes.Stelem_R4));

        public TReturn Stelem_R8() =>
            _func(_il.Create(OpCodes.Stelem_R8));

        public TReturn Stelem_Ref() =>
            _func(_il.Create(OpCodes.Stelem_Ref));

        public TReturn Ldelem_Any() =>
            _func(_il.Create(OpCodes.Ldelem_Any));

        public TReturn Stelem_Any() =>
            _func(_il.Create(OpCodes.Stelem_Any));

        public TReturn Unbox_Any() =>
            _func(_il.Create(OpCodes.Unbox_Any));

        public TReturn Conv_Ovf_I1() =>
            _func(_il.Create(OpCodes.Conv_Ovf_I1));

        public TReturn Conv_Ovf_U1() =>
            _func(_il.Create(OpCodes.Conv_Ovf_U1));

        public TReturn Conv_Ovf_I2() =>
            _func(_il.Create(OpCodes.Conv_Ovf_I2));

        public TReturn Conv_Ovf_U2() =>
            _func(_il.Create(OpCodes.Conv_Ovf_U2));

        public TReturn Conv_Ovf_I4() =>
            _func(_il.Create(OpCodes.Conv_Ovf_I4));

        public TReturn Conv_Ovf_U4() =>
            _func(_il.Create(OpCodes.Conv_Ovf_U4));

        public TReturn Conv_Ovf_I8() =>
            _func(_il.Create(OpCodes.Conv_Ovf_I8));

        public TReturn Conv_Ovf_U8() =>
            _func(_il.Create(OpCodes.Conv_Ovf_U8));

        public TReturn Refanyval() =>
            _func(_il.Create(OpCodes.Refanyval));

        public TReturn Ckfinite() =>
            _func(_il.Create(OpCodes.Ckfinite));

        public TReturn Mkrefany() =>
            _func(_il.Create(OpCodes.Mkrefany));

        public TReturn Ldtoken() =>
            _func(_il.Create(OpCodes.Ldtoken));

        public TReturn Conv_U2() =>
            _func(_il.Create(OpCodes.Conv_U2));

        public TReturn Conv_U1() =>
            _func(_il.Create(OpCodes.Conv_U1));

        public TReturn Conv_I() =>
            _func(_il.Create(OpCodes.Conv_I));

        public TReturn Conv_Ovf_I() =>
            _func(_il.Create(OpCodes.Conv_Ovf_I));

        public TReturn Conv_Ovf_U() =>
            _func(_il.Create(OpCodes.Conv_Ovf_U));

        public TReturn Add_Ovf() =>
            _func(_il.Create(OpCodes.Add_Ovf));

        public TReturn Add_Ovf_Un() =>
            _func(_il.Create(OpCodes.Add_Ovf_Un));

        public TReturn Mul_Ovf() =>
            _func(_il.Create(OpCodes.Mul_Ovf));

        public TReturn Mul_Ovf_Un() =>
            _func(_il.Create(OpCodes.Mul_Ovf_Un));

        public TReturn Sub_Ovf() =>
            _func(_il.Create(OpCodes.Sub_Ovf));

        public TReturn Sub_Ovf_Un() =>
            _func(_il.Create(OpCodes.Sub_Ovf_Un));

        public TReturn Endfinally() =>
            _func(_il.Create(OpCodes.Endfinally));

        public TReturn Leave() =>
            _func(_il.Create(OpCodes.Leave));

        public TReturn Leave_S() =>
            _func(_il.Create(OpCodes.Leave_S));

        public TReturn Stind_I() =>
            _func(_il.Create(OpCodes.Stind_I));

        public TReturn Conv_U() =>
            _func(_il.Create(OpCodes.Conv_U));

        public TReturn Arglist() =>
            _func(_il.Create(OpCodes.Arglist));

        public TReturn Ceq() =>
            _func(_il.Create(OpCodes.Ceq));

        public TReturn Cgt() =>
            _func(_il.Create(OpCodes.Cgt));

        public TReturn Cgt_Un() =>
            _func(_il.Create(OpCodes.Cgt_Un));

        public TReturn Clt() =>
            _func(_il.Create(OpCodes.Clt));

        public TReturn Clt_Un() =>
            _func(_il.Create(OpCodes.Clt_Un));

        public TReturn Ldftn() =>
            _func(_il.Create(OpCodes.Ldftn));

        public TReturn Ldvirtftn() =>
            _func(_il.Create(OpCodes.Ldvirtftn));

        public TReturn Ldarg() =>
            _func(_il.Create(OpCodes.Ldarg));

        public TReturn Ldarga() =>
            _func(_il.Create(OpCodes.Ldarga));

        public TReturn Starg() =>
            _func(_il.Create(OpCodes.Starg));

        public TReturn Ldloc() =>
            _func(_il.Create(OpCodes.Ldloc));

        public TReturn Ldloca() =>
            _func(_il.Create(OpCodes.Ldloca));

        public TReturn Stloc() =>
            _func(_il.Create(OpCodes.Stloc));

        public TReturn Localloc() =>
            _func(_il.Create(OpCodes.Localloc));

        public TReturn Endfilter() =>
            _func(_il.Create(OpCodes.Endfilter));

        public TReturn Unaligned() =>
            _func(_il.Create(OpCodes.Unaligned));

        public TReturn Volatile() =>
            _func(_il.Create(OpCodes.Volatile));

        public TReturn Tail() =>
            _func(_il.Create(OpCodes.Tail));

        public TReturn Initobj() =>
            _func(_il.Create(OpCodes.Initobj));

        public TReturn Constrained() =>
            _func(_il.Create(OpCodes.Constrained));

        public TReturn Cpblk() =>
            _func(_il.Create(OpCodes.Cpblk));

        public TReturn Initblk() =>
            _func(_il.Create(OpCodes.Initblk));

        public TReturn No() =>
            _func(_il.Create(OpCodes.No));

        public TReturn Rethrow() =>
            _func(_il.Create(OpCodes.Rethrow));

        public TReturn Sizeof() =>
            _func(_il.Create(OpCodes.Sizeof));

        public TReturn Refanytype() =>
            _func(_il.Create(OpCodes.Refanytype));

        public TReturn Readonly() =>
            _func(_il.Create(OpCodes.Readonly));
    }
}