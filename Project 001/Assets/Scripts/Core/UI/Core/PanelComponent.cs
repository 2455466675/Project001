using Game.Cfg;
using Game.Core;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class PanelComponent : EC.Component
    {
        public UIDefine.Panel_ID PanelID { get; private set; }

        public int Id { get; private set; }

        private Panel panel;
        public UIGroup UIGroup => panel.group;

        public void Init(UIDefine.Panel_ID panelID) 
        {
            PanelID = panelID;
            var parent = Entity.Parent.GetComponent<UIRootComponent>().GetWinGroup(UIGroup.Normal);
            var config = World.GetComponent<ConfigComponent>().Find<PanelCfg>((int)panelID);
            var panelGo = World.GetComponent<ResourceComponent>().LoadAndInstantiate(config.Path, parent.transform);

            panel = panelGo.GetComponent<Panel>();       
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
