using Game.Core;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class PanelComponent : EC.Component
    {
        public int Id { get; private set; }

        private Panel panel;
        public UIGroup UIGroup => panel.group;

        private List<NavigationGroupComponent> groupComponents;

        public void Init(int id) 
        {
            Id = id;

            var parent = MyEntity.Parent.GetComponent<UIRootComponent>().GetWinGroup(UIGroup.Normal);
            var panelGo = MyWorld.GetComponent<ResourceComponent>().LoadAndInstantiate("Assets/Bundles/UI/Panel/PanelTest", parent.transform);

            panel = panelGo.GetComponent<Panel>();

            var groups = panel.groups;
            if(groups != null && groups.Length > 0)
            {
                groupComponents = new List<NavigationGroupComponent>(groups.Length);
                for (int i = 0; i < groups.Length; i++)
                {
                    var group = groups[i];
                    var groupEntity = MyEntity.CreateChild();
                    var groupComponent = groupEntity.AddComponent<NavigationGroupComponent>();
                    groupComponent.Init(group);
                    groupComponents.Add(groupComponent);
                }
            }
        }

        public NavigationGroupComponent GetNavigationGroup(int index)
        {
            if (groupComponents == null)
            {
                return null;
            }

            return groupComponents[index];
        }


        public void Show() 
        {
            panel.Show();
        }

        public void Hide() 
        {
            panel.Hide();
        }

        public void InFocus() 
        {
        
        }

        public void OutFocus()
        {
        
        }

        public void Move() 
        {
        
        }

        public void Submit() 
        { 
        
        }

        protected override void OnDestroy()
        {
            GoHelper.DestroyGameObject(panel.gameObject);
            panel = null;
            groupComponents.Clear();
        }
    }
}
