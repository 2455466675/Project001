using EC;
using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class UIRootComponent : EC.Component, IInitializable
    {
        private UIRoot UIRoot;

        private List<PanelComponent> panels;

        public IEnumerator Init(GameInitCfg intCfg)
        {
            GameObject uiRootGo = World.GetComponent<ResourceComponent>().LoadAndInstantiate(intCfg.UIRootPath, null);
            UIRoot = uiRootGo.GetComponent<UIRoot>();

            panels = new List<PanelComponent>();

            yield return UIRoot;
        }

        public void ShowPanel(int id) 
        {
            PanelComponent pc = panels.Find(p => p.Id == id);
            if (pc != null) 
            {
                pc.Show();
            }
            else
            {
                Entity panelEntity = Entity.CreateChild();
                pc = panelEntity.AddComponent<PanelComponent>();
                pc.Init(id);
                pc.Show();
                panels.Add(pc);
            }
        }

        public void HidePanel(int id) 
        {
            PanelComponent pc = panels.Find(p => p.Id == id);
            if(pc != null) 
            {
                pc.Hide();
            }
        }

        public WindowGroup GetWinGroup(UIGroup group) 
        { 
            return UIRoot.GetWinGroup(group);
        }
    }
}
