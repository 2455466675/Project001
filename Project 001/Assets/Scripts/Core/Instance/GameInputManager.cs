using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class GameInputManager : MonoBehaviour, ICore
    {
        private MyInput inputActions;

        private List<InputActionBase> actions = new List<InputActionBase>();
        private List<InputActionBase> continuedActions = new List<InputActionBase>();

        public IEnumerator Init()
        {
            inputActions = new MyInput();
            inputActions.Enable();
            yield return null;
        }

        public void Start()
        {         
            actions.Add(new UIMoveAction(inputActions.UI.Move));
            actions.Add(new UICancelAction(inputActions.UI.Cancel));
            actions.Add(new UISubmitAction(inputActions.UI.Submit));
            actions.Add(new UICloseAction(inputActions.UI.Close));

            actions.Add(new RoleMoveAction(inputActions.Role.Move));
            actions.Add(new RoleMenuAction(inputActions.Role.Menu));
            actions.Add(new RoleAddSpeedAction(inputActions.Role.AddSpeed));
            DisableUIAction();
            DisableRoleAction();
        }

        public void EnableUIAction()
        {
            if (inputActions == null) 
            {
                return;
            }
      
            inputActions.UI.Enable();          
        }

        public void DisableUIAction()
        {
            if (inputActions == null)
            {
                return;
            }

            inputActions.UI.Disable();
        }

        public void EnableRoleAction()
        {
            if (inputActions == null)
            {
                return;
            }

            inputActions.Role.Enable();
        }

        public void DisableRoleAction()
        {
            if (inputActions == null)
            {
                return;
            }

            inputActions.Role.Disable();
        }

        public void PushAction(InputActionBase action)
        {
            continuedActions.Add(action);
        }

        public void PopAction(InputActionBase action) 
        {
            continuedActions.Remove(action);
        }

        public void FixedUpdate()
        {
            for (int i = 0; i < continuedActions.Count; i++)
            {
                continuedActions[i].Update();
            }
        }    
    }
}

