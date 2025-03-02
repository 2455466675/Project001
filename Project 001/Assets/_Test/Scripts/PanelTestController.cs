using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class PanelTestController : MonoBehaviour
    {
        public LoopNavigationGroup group_1;
        public FixedNavigationGroup group_2;

        private void Awake()
        {
            group_1.Init();
            group_2.Init();

            group_1.UpdateElementCount(10);
            group_2.UpdateElementCount(5);
        }

        public void OnGroup1Submit(NavigationItem item) 
        {
            MLog.Log("OnGroup1Submit");
            GameWorld.Instance.GetComponent<UIComponent>().Navigate(UIDefine.Group_ID.Test_Group_2);
        }

        public void OnGroup2Submit(NavigationItem item)
        {
            MLog.Log("OnGroup2Submit");
            GameWorld.Instance.GetComponent<UIComponent>().Navigate(UIDefine.Group_ID.Test_Group_3);
        }
    }
}
