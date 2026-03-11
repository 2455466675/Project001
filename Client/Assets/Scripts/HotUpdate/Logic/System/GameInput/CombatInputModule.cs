using GameFramework.Core;

namespace GameFramework.Logic
{
    [GameInputModule]
    public class CombatInputModule : InputModuleBase
    {
        public override InputModuleType ModuleType => InputModuleType.Combat;

        public override void OnInput(InputContext context)
        {
            //TODO 
        }
    }
}