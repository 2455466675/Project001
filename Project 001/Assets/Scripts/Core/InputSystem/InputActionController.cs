using System.Collections;
using System.Collections.Generic;

namespace Game.Core
{
    public enum InputMode
    {
        GM,
        Role,
        UI,
        Fight,
        Dialog,
    }

    public interface IInputActionController
    {
        InputMode Mode { get; }

        void Enable();
        void Disable();
    }

    /// <summary>
    /// 
    /// </summary>
	public abstract class InputActionController : IInputActionController
    {
        public abstract InputMode Mode { get;}

        protected MyInput inputActions;
        protected List<InputActionWrapper> actions;
        public InputActionController(MyInput inputActions)
        {
            this.inputActions = inputActions;
        }
        public abstract void Enable();

        public abstract void Disable();       
    }
}

