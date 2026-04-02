using GameFramework.Core;
using UnityEngine.XR;

namespace GameFramework.Logic
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
                    break;
            }
        }
    }
}