using EC;
using Game.UI;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class UIComponent : EC.Component, IInitializable
    {
        private Entity UIEntity;

        public IEnumerator Init(GameInitCfg intCfg) 
        {            
            UIEntity = Entity.CreateChild();

            UIRootComponent uirc = UIEntity.AddComponent<UIRootComponent>();

            yield return uirc.Init(intCfg);
        }

        public void ShowPanel(UIDefine.Panel_ID panelID)
        {
            UIEntity.GetComponent<UIRootComponent>().ShowPanel(panelID);
        }

        public void HidePanel(UIDefine.Panel_ID panelID)
        {
            UIEntity.GetComponent<UIRootComponent>().HidePanel(panelID);
        }

        public void Navigate(UIDefine.Group_ID groupID)
        {
            UIEntity.GetComponent<UIRootComponent>().Navigate(groupID);
        }

        public void Move(Vector2 dir) 
        {
            UIEntity.GetComponent<UIRootComponent>().Move(dir);
        }

        public void Submit()
        {
            UIEntity.GetComponent<UIRootComponent>().Submit();
        }

        public void Back()
        {
            UIEntity.GetComponent<UIRootComponent>().Back();
        }

        public void Close(bool compulsory = false)
        {
            UIEntity.GetComponent<UIRootComponent>().Close(compulsory);
        }

        public void AddNavigationGroup(NavigationGroup group)
        {
            UIEntity.GetComponent<UIRootComponent>().AddNavigationGroup(group);
        }

        public void RemoveNavigationGroup(UIDefine.Group_ID groupID)
        {
            UIEntity.GetComponent<UIRootComponent>().RemoveNavigationGroup(groupID);
        }

        public NavigationGroupComponent GetNavigationGroup(UIDefine.Group_ID groupID) 
        {
            return UIEntity.GetComponent<UIRootComponent>().GetNavigationGroup(groupID);
        }
    }
}
