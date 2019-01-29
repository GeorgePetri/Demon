using Microsoft.Build.Utilities;

namespace DemonWeaver
{
    public class Demon : Task
    {
        public override bool Execute()
        {

//            var module = ModuleDefinition.ReadModule(@".\bin\Debug\netcoreapp2.1\AssemblyToProcess.dll");

//            Log.LogWarning(module.Name);

            return true;
        }
    }
}