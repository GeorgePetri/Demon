namespace TestsWeaving.Helpers
{
    public class Helpers
    {
        public static string TestDataFilename
        {
            get
            {
                string configuration;

#if DEBUG
                configuration = "Debug";
#else
                configuration = "Release";
#endif
                return $@"..\..\..\..\TestDataForWeaving\bin\{configuration}\netcoreapp2.1\TestDataForWeaving.dll";
            }
        }
    }
}