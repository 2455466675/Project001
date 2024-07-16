using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

        public void Update()
        {
            //if (Input.GetKeyDown(KeyCode.K))
            //{
            //    inputActions.UI.Disable();
            //}
            //if (Input.GetKeyDown(KeyCode.L))
            //{
            //    inputActions.UI.Enable();
            //}

            for (int i = 0; i < continuedActions.Count; i++)
            {
                continuedActions[i].Update();
            }
        }

        public void FixedUpdate()
        {
            //float x = Input.GetAxisRaw("Horizontal");
            //float y = Input.GetAxisRaw("Vertical");
            //float s = Input.GetAxisRaw("Submit");
            //float c = Input.GetAxisRaw("Cancel");

            //t = Mathf.Max(0f, t - Time.deltaTime);
          
            //if (x != 0)
            //{
            //    if (t <= 0f)
            //    {
            //        //Debug.Log($"x:{x}");
            //        //GameCore.UI.TestH(x);
            //        t = 0.2f;
            //    }
            //}
            //if (y != 0)
            //{
            //    if (t <= 0f) 
            //    {
            //        //Debug.Log($"y:{y}");
            //        //GameCore.UI.TestV(y);
            //        t = 0.2f;
            //    }
            //}
            //if (s != 0)
            //{
            //    if (t <= 0f) 
            //    {
            //        MLog.Log($"s:{s}");
            //        //GameCore.UI.Submit();
            //        t = 0.2f;
            //    }
            //}
            //if (c != 0)
            //{
            //    if (t <= 0f)
            //    {
            //        MLog.Log($"c:{c}");
            //        if (GameCore.StateController.IsUIModel)
            //        {
            //            GameCore.UI.UndoCommand();
            //        }
            //        else
            //        {
            //            GameCore.UI.OpenWin(100003);
            //        }
            //        t = 0.2f;
            //    }
            //}

        }
    }
}

