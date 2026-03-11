using GameFramework.Core;

namespace GameFramework.Logic
{
    [GameInputModule]
    public class NormalInputModule : InputModuleBase
    {
        public override InputModuleType ModuleType => InputModuleType.Normal;

        public override void OnInput(InputContext context)
        {
            //TODO 
        }
    }
}