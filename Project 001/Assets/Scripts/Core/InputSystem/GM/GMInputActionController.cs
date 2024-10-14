using System.Collections;
using System.Collections.Generic;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
	public class GMInputActionController : InputActionController
    {
        public override InputMode Mode => InputMode.GM;
        public GMInputActionController(MyInput inputActions) : base(inputActions)
        {
            actions = new List<InputActionWrapper>();

            actions.Add(new GMAction(inputActions.GM.GM));
        }
        public override void Enable()
        {
            inputActions.GM.Enable();
        }

        public override void Disable()
        {
            inputActions.GM.Disable();
        }

    }
}

