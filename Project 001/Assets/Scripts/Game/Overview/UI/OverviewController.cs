using Game.Core;
using Game.System;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class OverviewController : MonoBehaviour
	{
        [SerializeField]
        private FixedNavigationGroup menuList;

        private void Awake()
        {
            menuList.Init();                        
        }

        private void OnEnable()
        {
            var list = GameWorld.Instance.GetComponent<SystemComponent>().OverviewComponent.GetMenuList();

            menuList.UpdateElementCount(list.Count);

            for (int i = 0; i < list.Count; i++)
            {
                NavigationItem item = menuList.GetItem(i);
                if (item != null) 
                {
                    item.SetData(list[i]);
                }
            }
        }

        public void OnClickMenu(NavigationItem item)
        {
            GameWorld.Instance.GetComponent<UIComponent>().Navigate(UI.UIDefine.Group_ID.Package_Menu_Group);
        }
    } 
}

