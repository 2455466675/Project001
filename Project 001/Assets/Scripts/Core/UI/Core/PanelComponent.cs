using Game.Cfg;
using Game.Core;
using System.Collections.Generic;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class PanelComponent : EC.Component
    {
        public UIDefine.Panel_ID PanelID => panel.panelID;

        public int Id { get; private set; }

        private Panel panel;
        public UIGroup UIGroup => panel.group;

        private List<NavigationGroupComponent> groupComponents;

        public void Init(UIDefine.Panel_ID panelID) 
        {
            var parent = MyEntity.Parent.GetComponent<UIRootComponent>().GetWinGroup(UIGroup.Normal);
            var config = MyWorld.GetComponent<ConfigComponent>().Find<PanelCfg>((int)panelID);
            var panelGo = MyWorld.GetComponent<ResourceComponent>().LoadAndInstantiate(config.Path, parent.transform);

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

        public NavigationGroupComponent GetNavigationGroup(UIDefine.Group_ID groupID)
        {
            if (groupComponents == null)
            {
                return null;
            }

            return groupComponents.Find(g => g.GroupID == groupID);
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
            groupComponents.Clear();
        }
    }
}
