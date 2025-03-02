using System.Collections.Generic;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
	public class UIInputActionController : InputActionController
    {
        public override InputMode Mode => InputMode.UI;
        public UIInputActionController(MyInput inputActions) : base(inputActions)
        {
            actions = new List<InputActionWrapper>
            {
                new UIMoveAction(inputActions.UI.Move),
                new UICancelAction(inputActions.UI.Cancel),
                new UISubmitAction(inputActions.UI.Submit),
                new UICloseAction(inputActions.UI.Close)
            };
        }
        public override void Enable()
        {
            inputActions.UI.Enable();
        }

        public override void Disable()
        {
            inputActions.UI.Disable();
        }

    }
}

