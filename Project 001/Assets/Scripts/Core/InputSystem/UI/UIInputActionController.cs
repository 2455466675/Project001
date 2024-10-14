using System.Collections;
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
            actions = new List<InputActionWrapper>();

            actions.Add(new UIMoveAction(inputActions.UI.Move));
            actions.Add(new UICancelAction(inputActions.UI.Cancel));
            actions.Add(new UISubmitAction(inputActions.UI.Submit));
            actions.Add(new UICloseAction(inputActions.UI.Close));
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

