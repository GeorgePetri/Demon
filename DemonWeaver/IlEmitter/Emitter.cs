using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace DemonWeaver.IlEmitter
{
    public class Emitter
    {
        readonly ILProcessor _il;
        readonly Action<Instruction> _func;

        public Emitter(ILProcessor il, Action<Instruction> func) => (_il, _func) = (il, func);

        public void Nop() =>
            _func(_il.Create(OpCodes.Nop));

        public void Break() =>
            _func(_il.Create(OpCodes.Break));

        public void Ldarg_0() =>
            _func(_il.Create(OpCodes.Ldarg_0));

        public void Ldarg_1() =>
            _func(_il.Create(OpCodes.Ldarg_1));

        public void Ldarg_2() =>
            _func(_il.Create(OpCodes.Ldarg_2));

        public void Ldarg_3() =>
            _func(_il.Create(OpCodes.Ldarg_3));

        public void Ldloc_0() =>
            _func(_il.Create(OpCodes.Ldloc_0));

        public void Ldloc_1() =>
            _func(_il.Create(OpCodes.Ldloc_1));

        public void Ldloc_2() =>
            _func(_il.Create(OpCodes.Ldloc_2));

        public void Ldloc_3() =>
            _func(_il.Create(OpCodes.Ldloc_3));

        public void Stloc_0() =>
            _func(_il.Create(OpCodes.Stloc_0));

        public void Stloc_1() =>
            _func(_il.Create(OpCodes.Stloc_1));

        public void Stloc_2() =>
            _func(_il.Create(OpCodes.Stloc_2));

        public void Stloc_3() =>
            _func(_il.Create(OpCodes.Stloc_3));

        public void Ldarg_S() =>
            _func(_il.Create(OpCodes.Ldarg_S));

        public void Ldarga_S() =>
            _func(_il.Create(OpCodes.Ldarga_S));

        public void Starg_S() =>
            _func(_il.Create(OpCodes.Starg_S));

        public void Ldloc_S() =>
            _func(_il.Create(OpCodes.Ldloc_S));

        public void Ldloca_S() =>
            _func(_il.Create(OpCodes.Ldloca_S));

        public void Stloc_S() =>
            _func(_il.Create(OpCodes.Stloc_S));

        public void Ldnull() =>
            _func(_il.Create(OpCodes.Ldnull));

        public void Ldc_I4_M1() =>
            _func(_il.Create(OpCodes.Ldc_I4_M1));

        public void Ldc_I4_0() =>
            _func(_il.Create(OpCodes.Ldc_I4_0));

        public void Ldc_I4_1() =>
            _func(_il.Create(OpCodes.Ldc_I4_1));

        public void Ldc_I4_2() =>
            _func(_il.Create(OpCodes.Ldc_I4_2));

        public void Ldc_I4_3() =>
            _func(_il.Create(OpCodes.Ldc_I4_3));

        public void Ldc_I4_4() =>
            _func(_il.Create(OpCodes.Ldc_I4_4));

        public void Ldc_I4_5() =>
            _func(_il.Create(OpCodes.Ldc_I4_5));

        public void Ldc_I4_6() =>
            _func(_il.Create(OpCodes.Ldc_I4_6));

        public void Ldc_I4_7() =>
            _func(_il.Create(OpCodes.Ldc_I4_7));

        public void Ldc_I4_8() =>
            _func(_il.Create(OpCodes.Ldc_I4_8));

        public void Ldc_I4_S() =>
            _func(_il.Create(OpCodes.Ldc_I4_S));

        public void Ldc_I4() =>
            _func(_il.Create(OpCodes.Ldc_I4));

        public void Ldc_I8() =>
            _func(_il.Create(OpCodes.Ldc_I8));

        public void Ldc_R4() =>
            _func(_il.Create(OpCodes.Ldc_R4));

        public void Ldc_R8() =>
            _func(_il.Create(OpCodes.Ldc_R8));

        public void Dup() =>
            _func(_il.Create(OpCodes.Dup));

        public void Pop() =>
            _func(_il.Create(OpCodes.Pop));

        public void Jmp() =>
            _func(_il.Create(OpCodes.Jmp));

        public void Call(MethodReference method) =>
            _func(_il.Create(OpCodes.Call, method));

        public void Calli() =>
            _func(_il.Create(OpCodes.Calli));

        public void Ret() =>
            _func(_il.Create(OpCodes.Ret));

        public void Br_S() =>
            _func(_il.Create(OpCodes.Br_S));

        public void Brfalse_S() =>
            _func(_il.Create(OpCodes.Brfalse_S));

        public void Brtrue_S() =>
            _func(_il.Create(OpCodes.Brtrue_S));

        public void Beq_S() =>
            _func(_il.Create(OpCodes.Beq_S));

        public void Bge_S() =>
            _func(_il.Create(OpCodes.Bge_S));

        public void Bgt_S() =>
            _func(_il.Create(OpCodes.Bgt_S));

        public void Ble_S() =>
            _func(_il.Create(OpCodes.Ble_S));

        public void Blt_S() =>
            _func(_il.Create(OpCodes.Blt_S));

        public void Bne_Un_S() =>
            _func(_il.Create(OpCodes.Bne_Un_S));

        public void Bge_Un_S() =>
            _func(_il.Create(OpCodes.Bge_Un_S));

        public void Bgt_Un_S() =>
            _func(_il.Create(OpCodes.Bgt_Un_S));

        public void Ble_Un_S() =>
            _func(_il.Create(OpCodes.Ble_Un_S));

        public void Blt_Un_S() =>
            _func(_il.Create(OpCodes.Blt_Un_S));

        public void Br() =>
            _func(_il.Create(OpCodes.Br));

        public void Brfalse() =>
            _func(_il.Create(OpCodes.Brfalse));

        public void Brtrue() =>
            _func(_il.Create(OpCodes.Brtrue));

        public void Beq() =>
            _func(_il.Create(OpCodes.Beq));

        public void Bge() =>
            _func(_il.Create(OpCodes.Bge));

        public void Bgt() =>
            _func(_il.Create(OpCodes.Bgt));

        public void Ble() =>
            _func(_il.Create(OpCodes.Ble));

        public void Blt() =>
            _func(_il.Create(OpCodes.Blt));

        public void Bne_Un() =>
            _func(_il.Create(OpCodes.Bne_Un));

        public void Bge_Un() =>
            _func(_il.Create(OpCodes.Bge_Un));

        public void Bgt_Un() =>
            _func(_il.Create(OpCodes.Bgt_Un));

        public void Ble_Un() =>
            _func(_il.Create(OpCodes.Ble_Un));

        public void Blt_Un() =>
            _func(_il.Create(OpCodes.Blt_Un));

        public void Switch() =>
            _func(_il.Create(OpCodes.Switch));

        public void Ldind_I1() =>
            _func(_il.Create(OpCodes.Ldind_I1));

        public void Ldind_U1() =>
            _func(_il.Create(OpCodes.Ldind_U1));

        public void Ldind_I2() =>
            _func(_il.Create(OpCodes.Ldind_I2));

        public void Ldind_U2() =>
            _func(_il.Create(OpCodes.Ldind_U2));

        public void Ldind_I4() =>
            _func(_il.Create(OpCodes.Ldind_I4));

        public void Ldind_U4() =>
            _func(_il.Create(OpCodes.Ldind_U4));

        public void Ldind_I8() =>
            _func(_il.Create(OpCodes.Ldind_I8));

        public void Ldind_I() =>
            _func(_il.Create(OpCodes.Ldind_I));

        public void Ldind_R4() =>
            _func(_il.Create(OpCodes.Ldind_R4));

        public void Ldind_R8() =>
            _func(_il.Create(OpCodes.Ldind_R8));

        public void Ldind_Ref() =>
            _func(_il.Create(OpCodes.Ldind_Ref));

        public void Stind_Ref() =>
            _func(_il.Create(OpCodes.Stind_Ref));

        public void Stind_I1() =>
            _func(_il.Create(OpCodes.Stind_I1));

        public void Stind_I2() =>
            _func(_il.Create(OpCodes.Stind_I2));

        public void Stind_I4() =>
            _func(_il.Create(OpCodes.Stind_I4));

        public void Stind_I8() =>
            _func(_il.Create(OpCodes.Stind_I8));

        public void Stind_R4() =>
            _func(_il.Create(OpCodes.Stind_R4));

        public void Stind_R8() =>
            _func(_il.Create(OpCodes.Stind_R8));

        public void Add() =>
            _func(_il.Create(OpCodes.Add));

        public void Sub() =>
            _func(_il.Create(OpCodes.Sub));

        public void Mul() =>
            _func(_il.Create(OpCodes.Mul));

        public void Div() =>
            _func(_il.Create(OpCodes.Div));

        public void Div_Un() =>
            _func(_il.Create(OpCodes.Div_Un));

        public void Rem() =>
            _func(_il.Create(OpCodes.Rem));

        public void Rem_Un() =>
            _func(_il.Create(OpCodes.Rem_Un));

        public void And() =>
            _func(_il.Create(OpCodes.And));

        public void Or() =>
            _func(_il.Create(OpCodes.Or));

        public void Xor() =>
            _func(_il.Create(OpCodes.Xor));

        public void Shl() =>
            _func(_il.Create(OpCodes.Shl));

        public void Shr() =>
            _func(_il.Create(OpCodes.Shr));

        public void Shr_Un() =>
            _func(_il.Create(OpCodes.Shr_Un));

        public void Neg() =>
            _func(_il.Create(OpCodes.Neg));

        public void Not() =>
            _func(_il.Create(OpCodes.Not));

        public void Conv_I1() =>
            _func(_il.Create(OpCodes.Conv_I1));

        public void Conv_I2() =>
            _func(_il.Create(OpCodes.Conv_I2));

        public void Conv_I4() =>
            _func(_il.Create(OpCodes.Conv_I4));

        public void Conv_I8() =>
            _func(_il.Create(OpCodes.Conv_I8));

        public void Conv_R4() =>
            _func(_il.Create(OpCodes.Conv_R4));

        public void Conv_R8() =>
            _func(_il.Create(OpCodes.Conv_R8));

        public void Conv_U4() =>
            _func(_il.Create(OpCodes.Conv_U4));

        public void Conv_U8() =>
            _func(_il.Create(OpCodes.Conv_U8));

        public void Callvirt() =>
            _func(_il.Create(OpCodes.Callvirt));

        public void Cpobj() =>
            _func(_il.Create(OpCodes.Cpobj));

        public void Ldobj() =>
            _func(_il.Create(OpCodes.Ldobj));

        public void Ldstr(string value) =>
            _func(_il.Create(OpCodes.Ldstr, value));

        public void Newobj(MethodReference method) =>
            _func(_il.Create(OpCodes.Newobj, method));

        public void Castclass() =>
            _func(_il.Create(OpCodes.Castclass));

        public void Isinst() =>
            _func(_il.Create(OpCodes.Isinst));

        public void Conv_R_Un() =>
            _func(_il.Create(OpCodes.Conv_R_Un));

        public void Unbox() =>
            _func(_il.Create(OpCodes.Unbox));

        public void Throw() =>
            _func(_il.Create(OpCodes.Throw));

        public void Ldfld(FieldReference field) =>
            _func(_il.Create(OpCodes.Ldfld, field));

        public void Ldflda() =>
            _func(_il.Create(OpCodes.Ldflda));

        public void Stfld() =>
            _func(_il.Create(OpCodes.Stfld));

        public void Ldsfld() =>
            _func(_il.Create(OpCodes.Ldsfld));

        public void Ldsflda() =>
            _func(_il.Create(OpCodes.Ldsflda));

        public void Stsfld(FieldReference field) =>
            _func(_il.Create(OpCodes.Stsfld, field));

        public void Stobj() =>
            _func(_il.Create(OpCodes.Stobj));

        public void Conv_Ovf_I1_Un() =>
            _func(_il.Create(OpCodes.Conv_Ovf_I1_Un));

        public void Conv_Ovf_I2_Un() =>
            _func(_il.Create(OpCodes.Conv_Ovf_I2_Un));

        public void Conv_Ovf_I4_Un() =>
            _func(_il.Create(OpCodes.Conv_Ovf_I4_Un));

        public void Conv_Ovf_I8_Un() =>
            _func(_il.Create(OpCodes.Conv_Ovf_I8_Un));

        public void Conv_Ovf_U1_Un() =>
            _func(_il.Create(OpCodes.Conv_Ovf_U1_Un));

        public void Conv_Ovf_U2_Un() =>
            _func(_il.Create(OpCodes.Conv_Ovf_U2_Un));

        public void Conv_Ovf_U4_Un() =>
            _func(_il.Create(OpCodes.Conv_Ovf_U4_Un));

        public void Conv_Ovf_U8_Un() =>
            _func(_il.Create(OpCodes.Conv_Ovf_U8_Un));

        public void Conv_Ovf_I_Un() =>
            _func(_il.Create(OpCodes.Conv_Ovf_I_Un));

        public void Conv_Ovf_U_Un() =>
            _func(_il.Create(OpCodes.Conv_Ovf_U_Un));

        public void Box() =>
            _func(_il.Create(OpCodes.Box));

        public void Newarr() =>
            _func(_il.Create(OpCodes.Newarr));

        public void Ldlen() =>
            _func(_il.Create(OpCodes.Ldlen));

        public void Ldelema() =>
            _func(_il.Create(OpCodes.Ldelema));

        public void Ldelem_I1() =>
            _func(_il.Create(OpCodes.Ldelem_I1));

        public void Ldelem_U1() =>
            _func(_il.Create(OpCodes.Ldelem_U1));

        public void Ldelem_I2() =>
            _func(_il.Create(OpCodes.Ldelem_I2));

        public void Ldelem_U2() =>
            _func(_il.Create(OpCodes.Ldelem_U2));

        public void Ldelem_I4() =>
            _func(_il.Create(OpCodes.Ldelem_I4));

        public void Ldelem_U4() =>
            _func(_il.Create(OpCodes.Ldelem_U4));

        public void Ldelem_I8() =>
            _func(_il.Create(OpCodes.Ldelem_I8));

        public void Ldelem_I() =>
            _func(_il.Create(OpCodes.Ldelem_I));

        public void Ldelem_R4() =>
            _func(_il.Create(OpCodes.Ldelem_R4));

        public void Ldelem_R8() =>
            _func(_il.Create(OpCodes.Ldelem_R8));

        public void Ldelem_Ref() =>
            _func(_il.Create(OpCodes.Ldelem_Ref));

        public void Stelem_I() =>
            _func(_il.Create(OpCodes.Stelem_I));

        public void Stelem_I1() =>
            _func(_il.Create(OpCodes.Stelem_I1));

        public void Stelem_I2() =>
            _func(_il.Create(OpCodes.Stelem_I2));

        public void Stelem_I4() =>
            _func(_il.Create(OpCodes.Stelem_I4));

        public void Stelem_I8() =>
            _func(_il.Create(OpCodes.Stelem_I8));

        public void Stelem_R4() =>
            _func(_il.Create(OpCodes.Stelem_R4));

        public void Stelem_R8() =>
            _func(_il.Create(OpCodes.Stelem_R8));

        public void Stelem_Ref() =>
            _func(_il.Create(OpCodes.Stelem_Ref));

        public void Ldelem_Any() =>
            _func(_il.Create(OpCodes.Ldelem_Any));

        public void Stelem_Any() =>
            _func(_il.Create(OpCodes.Stelem_Any));

        public void Unbox_Any() =>
            _func(_il.Create(OpCodes.Unbox_Any));

        public void Conv_Ovf_I1() =>
            _func(_il.Create(OpCodes.Conv_Ovf_I1));

        public void Conv_Ovf_U1() =>
            _func(_il.Create(OpCodes.Conv_Ovf_U1));

        public void Conv_Ovf_I2() =>
            _func(_il.Create(OpCodes.Conv_Ovf_I2));

        public void Conv_Ovf_U2() =>
            _func(_il.Create(OpCodes.Conv_Ovf_U2));

        public void Conv_Ovf_I4() =>
            _func(_il.Create(OpCodes.Conv_Ovf_I4));

        public void Conv_Ovf_U4() =>
            _func(_il.Create(OpCodes.Conv_Ovf_U4));

        public void Conv_Ovf_I8() =>
            _func(_il.Create(OpCodes.Conv_Ovf_I8));

        public void Conv_Ovf_U8() =>
            _func(_il.Create(OpCodes.Conv_Ovf_U8));

        public void Refanyval() =>
            _func(_il.Create(OpCodes.Refanyval));

        public void Ckfinite() =>
            _func(_il.Create(OpCodes.Ckfinite));

        public void Mkrefany() =>
            _func(_il.Create(OpCodes.Mkrefany));

        public void Ldtoken() =>
            _func(_il.Create(OpCodes.Ldtoken));

        public void Conv_U2() =>
            _func(_il.Create(OpCodes.Conv_U2));

        public void Conv_U1() =>
            _func(_il.Create(OpCodes.Conv_U1));

        public void Conv_I() =>
            _func(_il.Create(OpCodes.Conv_I));

        public void Conv_Ovf_I() =>
            _func(_il.Create(OpCodes.Conv_Ovf_I));

        public void Conv_Ovf_U() =>
            _func(_il.Create(OpCodes.Conv_Ovf_U));

        public void Add_Ovf() =>
            _func(_il.Create(OpCodes.Add_Ovf));

        public void Add_Ovf_Un() =>
            _func(_il.Create(OpCodes.Add_Ovf_Un));

        public void Mul_Ovf() =>
            _func(_il.Create(OpCodes.Mul_Ovf));

        public void Mul_Ovf_Un() =>
            _func(_il.Create(OpCodes.Mul_Ovf_Un));

        public void Sub_Ovf() =>
            _func(_il.Create(OpCodes.Sub_Ovf));

        public void Sub_Ovf_Un() =>
            _func(_il.Create(OpCodes.Sub_Ovf_Un));

        public void Endfinally() =>
            _func(_il.Create(OpCodes.Endfinally));

        public void Leave() =>
            _func(_il.Create(OpCodes.Leave));

        public void Leave_S() =>
            _func(_il.Create(OpCodes.Leave_S));

        public void Stind_I() =>
            _func(_il.Create(OpCodes.Stind_I));

        public void Conv_U() =>
            _func(_il.Create(OpCodes.Conv_U));

        public void Arglist() =>
            _func(_il.Create(OpCodes.Arglist));

        public void Ceq() =>
            _func(_il.Create(OpCodes.Ceq));

        public void Cgt() =>
            _func(_il.Create(OpCodes.Cgt));

        public void Cgt_Un() =>
            _func(_il.Create(OpCodes.Cgt_Un));

        public void Clt() =>
            _func(_il.Create(OpCodes.Clt));

        public void Clt_Un() =>
            _func(_il.Create(OpCodes.Clt_Un));

        public void Ldftn() =>
            _func(_il.Create(OpCodes.Ldftn));

        public void Ldvirtftn() =>
            _func(_il.Create(OpCodes.Ldvirtftn));

        public void Ldarg() =>
            _func(_il.Create(OpCodes.Ldarg));

        public void Ldarga() =>
            _func(_il.Create(OpCodes.Ldarga));

        public void Starg() =>
            _func(_il.Create(OpCodes.Starg));

        public void Ldloc() =>
            _func(_il.Create(OpCodes.Ldloc));

        public void Ldloca() =>
            _func(_il.Create(OpCodes.Ldloca));

        public void Stloc() =>
            _func(_il.Create(OpCodes.Stloc));

        public void Localloc() =>
            _func(_il.Create(OpCodes.Localloc));

        public void Endfilter() =>
            _func(_il.Create(OpCodes.Endfilter));

        public void Unaligned() =>
            _func(_il.Create(OpCodes.Unaligned));

        public void Volatile() =>
            _func(_il.Create(OpCodes.Volatile));

        public void Tail() =>
            _func(_il.Create(OpCodes.Tail));

        public void Initobj() =>
            _func(_il.Create(OpCodes.Initobj));

        public void Constrained() =>
            _func(_il.Create(OpCodes.Constrained));

        public void Cpblk() =>
            _func(_il.Create(OpCodes.Cpblk));

        public void Initblk() =>
            _func(_il.Create(OpCodes.Initblk));

        public void No() =>
            _func(_il.Create(OpCodes.No));

        public void Rethrow() =>
            _func(_il.Create(OpCodes.Rethrow));

        public void Sizeof() =>
            _func(_il.Create(OpCodes.Sizeof));

        public void Refanytype() =>
            _func(_il.Create(OpCodes.Refanytype));

        public void Readonly() =>
            _func(_il.Create(OpCodes.Readonly));
    }
}