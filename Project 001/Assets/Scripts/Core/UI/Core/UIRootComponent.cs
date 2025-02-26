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
            GameObject uiRootGo = MyWorld.GetComponent<ResourceComponent>().LoadAndInstantiate(intCfg.UIRootPath, null);
            UIRoot = uiRootGo.GetComponent<UIRoot>();

            panels = new List<PanelComponent>();

            yield return UIRoot;
        }

        public void ShowPanel<T>(int id) where T : PanelControllerComponent, new()
        {
            PanelComponent pc = panels.Find(p => p.Id == id);
            if (pc != null) 
            {
                pc.Show();

                T controller = pc.GetComponent<T>();
                controller.Show();
            }
            else
            {
                Entity panelEntity = MyEntity.CreateChild();
                pc = panelEntity.AddComponent<PanelComponent>();
                T controller = panelEntity.AddComponent<T>();

                pc.Init(id);
                pc.Show();
                controller.Show();

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
