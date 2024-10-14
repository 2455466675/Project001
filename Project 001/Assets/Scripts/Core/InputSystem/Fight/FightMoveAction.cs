using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
	public class FightMoveAction : InputActionWrapper
    {
        private float intervalTime = 0.15f;
        private float t;
        public FightMoveAction(InputAction inputAction) : base(inputAction)
        {
        }

        public override void FixedUpdate()
        {
            if (t <= 0f)
            {
                Execute();
                t = intervalTime;
            }
            else
            {
                t -= Time.fixedDeltaTime;
            }
        }

        public override void OnStarted(InputAction.CallbackContext obj)
        {
            GameCore.InputSys.PushAction(this);
            Execute();
            t = intervalTime;
        }

        public override void OnPerformed(InputAction.CallbackContext obj)
        {
        }

        public override void OnCanceled(InputAction.CallbackContext obj)
        {
            GameCore.InputSys.PopAction(this);
        }

        public override void Execute()
        {
            MLog.Log("FightMoveAction");
        }
    }
}

