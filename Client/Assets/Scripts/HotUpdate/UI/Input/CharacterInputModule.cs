using GameFramework.Core;
using GameFramework.Featrue;
using GameFramework.Gameplay;

namespace GameFramework.UI
{
    public class CharacterCommand : InputCommand 
    {
        protected override bool CheckLocked()
        {
            return true;
        }

        protected override void OnInputAction(InputContext context)
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

    public class CharacterInputModule : InputModule
    {
        public override InputModuleType ModuleType => InputModuleType.Character;

        public CharacterInputModule()
        {
            Push(new CharacterCommand());
        }

        protected override void OnInputAction(InputContext context)
        {
            switch (context.Input)
            {
                case InputDefine.Move:
                case InputDefine.Move2:
                case InputDefine.LeftShift:
                    break;
                case InputDefine.Cancel:
                    Pop();
                    break;
                case InputDefine.Esc:
                    PopAll();
                    break;
            }
        }
    }
}
