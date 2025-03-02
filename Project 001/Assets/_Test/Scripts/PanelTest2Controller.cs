using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class PanelTest2Controller : MonoBehaviour
    {
        public StaticNavigationGroup group;

        private void Awake()
        {
            group.Init();
        }

        public void OnGroupSubmit(NavigationItem item) 
        {
            MLog.Log("PanelTest2Controller - OnGroupSubmit");
        }
    }
}
