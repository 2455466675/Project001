using GameFramework.Core;
using GameFramework.Gameplay;

namespace GameFramework.UI 
{
    [GameEvent]
    public class InputEvent_Handler : GameEventHandlerBase<InputEventArgs>
    {
        public override void Invoke(InputEventArgs arg)
        {
            if (arg.context.Input == InputDefine.M_Keyboard) 
            {
                Game.Gameplay.GetSystem<InventorySystem>().Test();
                Game.GetModule<SaveManager>().Save(1);
            }

            if (arg.context.Input == InputDefine.Cancel)
            {              
                Game.GetModule<SaveManager>().Load(1);
            }

            Game.GetModule<InputController>().InputAction(arg.context);
        }
    }
}