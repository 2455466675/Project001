using Cysharp.Threading.Tasks;

namespace GameFramework.Logic
{
    public class BattleSimulation
    {
        public void Start()
        {
            BattleContext context = new BattleContext();
            BattleFlow flow = new BattleFlow();
            flow.Run(context).Forget();
        }
    }
}
