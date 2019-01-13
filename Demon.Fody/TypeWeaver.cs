using Mono.Cecil;

namespace Demon.Fody
{
    //todo this call does all the aspect weaving for a type
    //todo recieve collection of aspects
    public class TypeWeaver
    {
        private readonly TypeDefinition _type;

        public TypeWeaver(TypeDefinition type) => _type = type;

        //todo
        public void Weave()
        {
            
        }
    }
}