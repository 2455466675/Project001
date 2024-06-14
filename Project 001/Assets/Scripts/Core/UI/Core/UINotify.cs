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

        private ListItem listItem;

        public void OnSubmit(ListItem listItem)
        {
            this.listItem = listItem;
            DoExecute(onSubmitFunName);
        }

        public void OnSelect(ListItem listItem)
        {
            this.listItem = listItem;
            DoExecute(onSelectFunName);
        }

        public void OnDeselect(ListItem listItem)
        {
            this.listItem = listItem;
            DoExecute(onDeselectFunName);
        }

        public void OnMoveToUp(ListItem listItem)
        {
            this.listItem = listItem;
            DoExecute(onMoveToUpFunName);
        }

        public void OnMoveToDown(ListItem listItem)
        {
            this.listItem = listItem;
            DoExecute(onMoveToDownFunName);
        }

        public void OnMoveToLeft(ListItem listItem)
        {
            this.listItem = listItem;
            DoExecute(onMoveToLeftFunName);
        }

        public void OnMoveToRight(ListItem listItem)
        {
            this.listItem = listItem;
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
            methodInfo.Invoke(controller, new object[] { new UINotification(this.listItem)});
        }
    }
}