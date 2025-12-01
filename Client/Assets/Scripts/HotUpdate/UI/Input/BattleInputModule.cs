using GameFramework.Core;
using GameFramework.Gameplay;

namespace GameFramework.UI
{
    public class BattleInputModule : InputModule
    {
        public override InputModuleType ModuleType => InputModuleType.Battle;

        public override void InputAction(InputContext context)
        {
            switch (context.Input)
            {
                case InputDefine.Move:
                case InputDefine.Submit:
                    if (navigateCammand.Count > 0)
                    {
                        navigateCammand.InputAction(context);
                    }
                    break;
                case InputDefine.Move2:
                    Game.GetModule<CameraManager>().Move(new UnityEngine.Vector3(context.X, 0, context.Y));
                    break;

                case InputDefine.LeftShift:
                    break;
                case InputDefine.Cancel:
                    break;
                case InputDefine.Esc:
                    Game.GetSystem<BattleSystem>().ExitBattle();
                    break;
                case InputDefine.M_Keyboard:
                    break;
                case InputDefine.Page:
                    Game.GetModule<CameraManager>().Rotate(new UnityEngine.Vector2(context.X, context.Y));
                    break;
            }
        }

        /* ActionCammand
         * 
         * SelectMovePosition
         *{
         *  SelectPosition
         *  {
                Navigate BattleGrid
                PushCammand
                Submit -> PopCammand
         *  }
         *  MoveToPosition
         *}
         * 
         *
         * SelectActionOption
         * 
         * SelectEffectArea
         * 
         *
         */
    }
}
