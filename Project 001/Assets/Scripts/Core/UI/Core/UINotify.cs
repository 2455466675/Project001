using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class UINotify : MonoBehaviour
    {
        public UIController controller;

        public string onSubmitFunName;
        public string onSelectFunName;
        public string onDeselectFunName;
        public string onMoveToUpFunName;
        public string onMoveToDownFunName;
        public string onMoveToLeftFunName;
        public string onMoveToRightFunName;

        private IGuidable guidable;

        public void OnSubmit(IGuidable guidable)
        {
            this.guidable = guidable;
            DoExecute(onSubmitFunName);
        }

        public void OnSelect(IGuidable guidable)
        {
            this.guidable = guidable;
            DoExecute(onSelectFunName);
        }

        public void OnDeselect(IGuidable guidable)
        {
            this.guidable = guidable;
            DoExecute(onDeselectFunName);
        }

        public void OnMoveToUp(IGuidable guidable)
        {
            this.guidable = guidable;
            DoExecute(onMoveToUpFunName);
        }

        public void OnMoveToDown(IGuidable guidable)
        {
            this.guidable = guidable;
            DoExecute(onMoveToDownFunName);
        }

        public void OnMoveToLeft(IGuidable guidable)
        {
            this.guidable = guidable;
            DoExecute(onMoveToLeftFunName);
        }

        public void OnMoveToRight(IGuidable guidable)
        {
            this.guidable = guidable;
            DoExecute(onMoveToRightFunName);
        }

        public void DoExecute(string funName)
        {
            if(string.IsNullOrEmpty(funName))
            {
                return;
            }
            if (controller == null)
            {
                return;
            }
            MethodInfo methodInfo = controller.GetType().GetMethod(funName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (methodInfo == null)
            {
                Debug.Log($"{controller.GetType().FullName}没有实现的函数：{funName}");
                return;
            }
            methodInfo.Invoke(controller, new object[] { new UINotification(this.guidable)});
        }
    }
}