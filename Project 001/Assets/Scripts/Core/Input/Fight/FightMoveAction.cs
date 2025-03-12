using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Core
{
    /// <summary>
    /// ’Ω∂∑ ‰»Î-“∆∂Ø
    /// </summary>
	public class FightMoveAction : InputActionWrapper
    {
        private readonly float pressTime = 0.3f;
        private float pressTimer;

        private readonly float intervalTime = 0.15f;
        private float intervalTimer;
        public FightMoveAction(InputAction inputAction) : base(inputAction)
        {
        }

        public override void Tick()
        {
            if (pressTimer > 0)
            {
                pressTimer -= Time.deltaTime;
                return;
            }

            if (intervalTimer <= 0f)
            {
                Execute();
                intervalTimer = intervalTime;
            }
            else
            {
                intervalTimer -= Time.deltaTime;
            }
        }

        public override void OnStarted(InputAction.CallbackContext obj)
        {
            Execute();
            pressTimer = pressTime;
            intervalTimer = intervalTime;
            PushContinued();
        }

        public override void OnPerformed(InputAction.CallbackContext obj)
        {
        }

        public override void OnCanceled(InputAction.CallbackContext obj)
        {
            PopContinued();
        }

        public override void Execute()
        {
            Vector2 dir = inputAction.ReadValue<Vector2>();
            GameWorld.Instance.GetComponent<UIComponent>().Move(dir);
        }
    }
}

