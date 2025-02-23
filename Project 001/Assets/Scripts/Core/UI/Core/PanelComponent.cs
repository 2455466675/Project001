using Game.Cfg;
using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class PanelComponent : EC.Component
    {
        public int Id { get; private set; }

        public UIGroup UIGroup => panel.group;

        private Panel panel;

        public void Init(int id) 
        {
            Id = id;

            var parent = Entity.Parent.GetComponent<UIRootComponent>().GetWinGroup(UIGroup.Normal);

            GameObject panelGo = World.GetComponent<ResourceComponent>().LoadAndInstantiate("Assets/Bundles/UI/Panel/PanelTest", parent.transform);

            panel = panelGo.GetComponent<Panel>();

            NavigationGroup[] groups = panel.groups;
            if(groups != null && groups.Length > 0) 
            {
                foreach (var group in groups)
                {
                    
                }
            }
        }

        public void Show() 
        {
            panel.Show();
        }

        public void Hide() 
        {
            panel.Hide();
        }

        protected override void OnDestroy()
        {
            GoHelper.DestroyGameObject(panel.gameObject);
            panel = null;
        }
    }
}
