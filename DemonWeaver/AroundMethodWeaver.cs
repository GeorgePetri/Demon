using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace DemonWeaver
{
    //todo impl and don't copy paste
    //todo either make another class with common or merge this and BeforeMethodWeaver
    public class AroundMethodWeaver
    {
        readonly DemonTypes _demonTypes;
        readonly MethodDefinition _target;
        readonly MethodReference _advice;
        readonly FieldDefinition _adviceField;
        readonly ILProcessor _il;
        readonly Instruction _originalFirstInstruction;

        public AroundMethodWeaver(DemonTypes demonTypes, MethodDefinition target, MethodReference advice, FieldDefinition adviceField)
        {
            _demonTypes = demonTypes;
            _target = target;
            _advice = advice;
            _adviceField = adviceField;
            _il = target.Body.GetILProcessor();
            _originalFirstInstruction = target.Body.Instructions[0];
        }

        public void Weave()
        {
            var joinPoint = GetJoinPoint();

            var joinPointType = (GenericInstanceType) joinPoint.ParameterType;

//            _target.Module.ImportReference(joinPointType);

            if (joinPointType.GetElementType().FullName == DemonTypes.FullNames.JoinPoint)
                WeaveSync(joinPointType);
        }

        //todo target should be sync, filter 
        void WeaveSync(GenericInstanceType joinPointType)
        {
            var ctor = MakeGeneric(_demonTypes.JoinPointConstructor, joinPointType.GenericArguments[0], joinPointType.GenericArguments[1]);

            _target.Body.Instructions.Clear();

            _il.Append(_il.Create(OpCodes.Ldnull));
            _il.Append(_il.Create(OpCodes.Ldnull));
            _il.Append(_il.Create(OpCodes.Newobj, _target.Module.ImportReference(ctor)));
            _il.Append(_il.Create(OpCodes.Ldarg_0));
            _il.Append(_il.Create(OpCodes.Ldfld, _adviceField));
            _il.Append(_il.Create(OpCodes.Call, _advice));
            _il.Append(_il.Create(OpCodes.Ldnull));
            _il.Append(_il.Create(OpCodes.Ret));
        }

        //todo allow a typejoinpoint, either inside the joinPoint or as a separate parameter in the aspect
        ParameterDefinition GetJoinPoint()
        {
            if (_advice.Parameters.Count == 1)
                return _advice.Parameters[0];
            throw new WeavingException("Around advice must have only one parameter, which is a JoinPoint"); //todo add context if missing, make nicer
        }

        //todo make extension method
        public static MethodReference MakeGeneric(MethodReference self, params TypeReference[] arguments)
        {
            var reference = new MethodReference(self.Name, self.ReturnType)
            {
                DeclaringType = MakeGenericType(self.DeclaringType, arguments),
                HasThis = self.HasThis,
                ExplicitThis = self.ExplicitThis,
                CallingConvention = self.CallingConvention,
            };

            foreach (var parameter in self.Parameters)
                reference.Parameters.Add(new ParameterDefinition(parameter.ParameterType));

            foreach (var generic_parameter in self.GenericParameters)
                reference.GenericParameters.Add(new GenericParameter(generic_parameter.Name, reference));

            return reference;
        }

        //todo make extension method

        public static MethodReference MakeGenericMethod(MethodReference self, params TypeReference[] arguments)
        {
            if (self.GenericParameters.Count != arguments.Length)
                throw new ArgumentException();

            var instance = new GenericInstanceMethod(self);
            foreach (var argument in arguments)
                instance.GenericArguments.Add(argument);

            return instance;
        }

        //todo make extension method
        public static TypeReference MakeGenericType(TypeReference self, params TypeReference[] arguments)
        {
            if (self.GenericParameters.Count != arguments.Length)
                throw new ArgumentException();

            var instance = new GenericInstanceType(self);
            foreach (var argument in arguments)
                instance.GenericArguments.Add(argument);

            return instance;
        }
    }
}