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
            SolutionWeaver.Weave(Assemblies);

            return true;
        }
    }
}