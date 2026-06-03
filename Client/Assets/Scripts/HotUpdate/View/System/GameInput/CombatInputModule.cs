using GameFramework.Core;
using GameFramework.Logic;

namespace GameFramework.View
{
    [GameInputModule]
    public class CombatInputModule : InputModuleBase
    {
        public override InputModuleType ModuleType => InputModuleType.Combat;

        public override void OnInput(InputContext context)
        {
            switch (context.Input)
            {
                case InputActionDefine.Move:
                case InputActionDefine.Move2:
                case InputActionDefine.LeftShift:
                    break;

                case InputActionDefine.Cancel:
                    Game.GetModule<BattleModule>().ExitBattle();
                    break;
                case InputActionDefine.M_Keyboard:
                    Game.Message.SendMessage(new UINavigationMessage() { navigation = Utility.GameDefine.NavigationDefine.BattlePlayer });
                    break;
            }
        }
    }
}