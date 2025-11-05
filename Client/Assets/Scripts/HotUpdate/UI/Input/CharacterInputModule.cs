using GameFramework.Core;
using GameFramework.Featrue;
using GameFramework.Gameplay;

namespace GameFramework.UI
{
    public class CharacterInputModule : InputModule
    {
        public override InputModuleType ModuleType => InputModuleType.Character;

        protected override void OnInputAction(InputContext context)
        {
            Entity leader = Game.Gameplay.GetSystem<PartySystem>().GetLeader();
            if (leader == null)
            {
                return;
            }

            var mc = leader.GetComponent<MotorComponent>();
            mc.OnInputAction(context);
        }
    }
}
