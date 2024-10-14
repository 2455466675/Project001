using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Core
{
    public class InputSystem : MonoBehaviour, ICore
    {
        private MyInput inputActions;

        private IInputActionController controller;
        private List<IInputActionController> controllers;
        private List<InputActionWrapper> continuedActions = new List<InputActionWrapper>();

        public IEnumerator Init()
        {
            inputActions = new MyInput();
            inputActions.Enable();

            controllers = new List<IInputActionController>
            {
                new UIInputActionController(inputActions),
                new RoleInputActionController(inputActions),
                new FightInputActionController(inputActions),
            };

            foreach (var item in controllers)
            {
                item.Disable();
            }

            yield return null;
        }

        public void SwitchInputMode(InputMode mode)
        {
            foreach (var item in controllers) 
            {
                if (item.Mode != mode)
                {
                    item.Disable();
                }
            }

            if (controller != null && controller.Mode == mode) 
            {
                return;
            }

            controller = controllers.Find(c => c.Mode == mode);
            controller.Enable();
        }

        public void PushAction(InputActionWrapper action)
        {
            continuedActions.Add(action);
        }

        public void PopAction(InputActionWrapper action) 
        {
            continuedActions.Remove(action);
        }

        public void FixedUpdate()
        {
            for (int i = 0; i < continuedActions.Count; i++)
            {
                continuedActions[i].FixedUpdate();
            }
        }    
    }
}

