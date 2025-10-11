using GameFramework.Core;

namespace GameFramework.UI 
{
    [GameEvent]
    public class InputEvent_Handler : GameEventHandlerBase<InputEventArgs>
    {
        public override void Invoke(InputEventArgs arg)
        {
            Game.GetModule<InputController>().InputAction(arg.context);
        }
    }
}