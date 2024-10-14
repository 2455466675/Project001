using System.Collections.Generic;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
	public class FightInputActionController : InputActionController
    {
        public override InputMode Mode => InputMode.Fight;

        public FightInputActionController(MyInput inputActions) : base(inputActions)
        {
            actions = new List<InputActionWrapper>();

            actions.Add(new FightMoveAction(inputActions.Fight.Move));
            actions.Add(new FightSubmitAction(inputActions.Fight.Submit));
            actions.Add(new FightCancelAction(inputActions.Fight.Cancel));
        }

        public override void Enable()
        {
            inputActions.Fight.Enable();
        }

        public override void Disable()
        {
            inputActions.Fight.Disable();
        }
    }
}

