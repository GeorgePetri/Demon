using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace DemonWeaver
{
    public class Demon : Task
    {
        [Required]
        public string[] Assemblies { get; set; }

        public override bool Execute()
        {
//            var module = ModuleDefinition.ReadModule(@".\bin\Debug\netcoreapp2.1\AssemblyToProcess.dll");


            foreach (var assembly in Assemblies)
            {
                Log.LogWarning(assembly);
            }

            return true;
        }
    }
}