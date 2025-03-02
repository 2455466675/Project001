using UnityEngine.InputSystem;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
	public class UICancelAction : InputActionWrapper
    {
        public UICancelAction(InputAction inputAction) : base(inputAction)
        {
        }

        public override void OnStarted(InputAction.CallbackContext obj)
        {
            Execute();
        }

        public override void Execute()
        {
            GameWorld.Instance.GetComponent<UIComponent>().Back();
        }
    }
}

