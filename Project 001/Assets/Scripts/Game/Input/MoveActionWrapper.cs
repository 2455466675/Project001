using ECS;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Input
{
    /// <summary>
    /// 
    /// </summary>
    public class MoveActionWrapper : InputActionWrapper, IUpdate
    {
        public event Action<float, float> ActionEvent;

        private readonly float pressTime = 0.3f;
        private float pressTimer;

        private readonly float intervalTime = 0.15f;
        private float intervalTimer;

        private bool isPress;

        public void Update(float dt)
        {
            if (!isPress) 
            {
                return;
            }

            if (pressTimer > 0)
            {
                pressTimer -= dt;
                return;
            }

            if (intervalTimer <= 0f)
            {
                Execute();
                intervalTimer = intervalTime;
            }
            else
            {
                intervalTimer -= dt;
            }
        }

        protected override void OnStarted(InputAction.CallbackContext obj)
        {
            Execute();
            pressTimer = pressTime;
            intervalTimer = intervalTime;
            isPress = true;
        }

        protected override void OnCanceled(InputAction.CallbackContext obj)
        {
            isPress = false;
        }

        private void Execute()
        {
            Vector2 v = inputAction.ReadValue<Vector2>();
            ActionEvent?.Invoke(v.x, v.y);
        }
    }
}
