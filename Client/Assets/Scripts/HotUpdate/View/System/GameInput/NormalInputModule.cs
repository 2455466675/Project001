using GameFramework.Core;
using GameFramework.Logic;

namespace GameFramework.View
{
    [GameInputModule]
    public class NormalInputModule : InputModuleBase
    {
        public override InputModuleType ModuleType => InputModuleType.Normal;

        public override void OnInput(InputContext context)
        {
            switch (context.Input)
            {
                case InputActionDefine.Move:
                case InputActionDefine.Move2:
                case InputActionDefine.LeftShift:
                    Game.GetModule<PartyModule>().OnInput(context);
                    break;

                case InputActionDefine.Cancel:
                    Game.GetModule<BattleModule>().EnterBattle();
                    break;
                case InputActionDefine.Esc:
                case InputActionDefine.M_Keyboard:
                    Game.GetSystem<GameSaveSystem>().SaveGame(0);
                    break;
            }
        }
    }
}