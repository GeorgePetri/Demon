using Demon.Aspect;

namespace AssemblyToProcess
{
    //todo define a nicer syntax than public void methods with empty body
    [Aspect]
    public class PointcutExpressions
    {
        [Pointcut("Within(AssemblyToProcess.Repositories.**)")]
        public void Repositories()
        {
        }

        [Pointcut("Execution(* **.Get*(**))")]
        public void Get()
        {
        }
        
        [Pointcut("Repositories() && Get()")]
        public void RepositoriesGet()
        {
        }
        
        [Pointcut("Repositories() && !Get()")]
        public void RepositoriesNonGet()
        {
        }
        
        [Pointcut("Execution(* **(Int))")]
        public void ExecutionIntParameter()
        {
        }
        
        [Pointcut("Execution(* **(Int,*))")]
        public void ExecutionIntAndOneAnyParameter()
        {
        }
        
        [Pointcut("Execution(* **(Int,**))")]
        public void ExecutionIntAndManyAnyParameter()
        {
        }
    }
}