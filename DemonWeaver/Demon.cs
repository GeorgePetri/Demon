using Microsoft.Build.Utilities;

namespace DemonWeaver
{
    public class Demon : Task
    {
        public override bool Execute()
        {
            Log.LogWarning("Demon Executed");

            return true;
        }
    }
}