using GameFramework.Core;
using GameFramework.Featrue;
using GameFramework.Gameplay;

namespace GameFramework.UI
{
    public class CharacterInputModule : InputModule
    {
        public override InputModuleType ModuleType => InputModuleType.Character;

        public override void InputAction(InputContext context)
        {
            if (navigateCammand.Count > 0)
            {
                navigateCammand.InputAction(context);
            }
            else
            {
                OnInputAction(context);
            }
        }

        private void OnInputAction(InputContext context)
        {
            Entity leader = Game.GetSystem<PartySystem>().GetLeader();
            if (leader == null)
            {
                return;
            }

            switch (context.Input)
            {
                case InputDefine.Move:
                case InputDefine.Move2:
                case InputDefine.LeftShift:
                    var mc = leader.GetComponent<MotorComponent>();
                    mc.OnInputAction(context);
                    break;

                case InputDefine.Cancel:
                    Game.GetSystem<BattleSystem>().EnterBattle();
                    break;
                case InputDefine.Esc:
                    break;
            }


        }
    }
}
