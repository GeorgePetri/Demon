using System.Collections.Generic;
using Demon.Fody.Data;
using Mono.Cecil;

namespace Demon.Fody
{
    //todo this call does all the aspect weaving for a type
    public class TypeWeaver
    {
        private readonly TypeDefinition _type;
        private readonly IEnumerable<AspectData> _aspects;

        public TypeWeaver(TypeDefinition type, IEnumerable<AspectData> aspects) => 
            (_type, _aspects) = (type, aspects);

        //todo
        public static void Weave(TypeDefinition type, IEnumerable<AspectData> aspects) =>
            new TypeWeaver(type, aspects).Weave();
        
        //todo
        private void Weave()
        {
            
        }
    }
}