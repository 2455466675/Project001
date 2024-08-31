using UnityEngine.InputSystem;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
	public class GMAction : InputActionBase
    {
        public GMAction(InputAction inputAction) : base(inputAction)
        {
        }

        public override void OnStarted(InputAction.CallbackContext obj)
        {
            Execute();
        }

        public override void Execute()
        {
            MLog.Log("GM");
            GameCore.UI.Enter(UI.WindowId.WinGM);
        }
    }
}

