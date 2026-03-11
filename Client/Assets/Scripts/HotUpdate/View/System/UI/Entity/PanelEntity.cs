using Config;
using GameFramework.Core;
using GameFramework.Utility;
using GameFramework.Utility.GameDefine;
using System.Collections.Generic;

namespace GameFramework.View.UI
{
    public class PanelEntity
    {
        private UIPanel panel;
        private IPanelController controller;

        private Dictionary<NavigationDefine, NavigationListEntity> navigationListEntities;

        public void Init(PanelDefine id)
        {
            var cfg = Game.ConfigManager.Find<PanelCfg>((int)id);
            var group = GameRoot.GetNode<UINode>().GetGroup(cfg.GroupType);
            var go = Game.ResourcesManager.LoadAndInstantiate(cfg.Path, group);
            panel = go.GetComponent<UIPanel>();

            controller = Game.GetSystem<UISystem>().GetPanelController(id);
            if (controller == null)
            {
                MDebug.Error("PanelController is null : ", id.ToString());
            }

            var views = panel.GetNavigationViews();
            var defines = Game.GetSystem<UISystem>().GetNavigationDefines(id);

            int length = Util.Math.Min(views.Length, defines.Length);
            navigationListEntities = new Dictionary<NavigationDefine, NavigationListEntity>(length);

            for (int i = 0; i < length; i++)
            {
                NavigationListEntity navigationList = new NavigationListEntity();
                navigationList.Init(views[i], defines[i]);
                navigationListEntities.Add(defines[i], navigationList);
            }
        }

        public void Destroy()
        {
            foreach (var item in navigationListEntities)
            {
                item.Value.Destroy();
            }
            navigationListEntities.Clear();
            panel = null;
            controller = null;
        }

        public NavigationListEntity GetNavigationListEntity(NavigationDefine navigationDefine)
        {
            if (navigationListEntities.ContainsKey(navigationDefine))
            {
                return navigationListEntities[navigationDefine];
            }
            else
            {
                return null;
            }
        }

        public void Show(object content)
        {
            controller?.Show(panel, content);
            foreach (var item in navigationListEntities)
            {
                item.Value.Show();
            }
        }

        public void Hide()
        {
            controller?.Hide();
            foreach (var item in navigationListEntities)
            {
                item.Value.Hide();
            }
        }

        public void Refocus()
        {
            controller?.Refocus();
        }

        public void OutFocus()
        {
            controller?.OutFocus();
        }

        public bool CheckLocked()
        {
            if (controller == null)
            {
                return false;
            }
            else
            {
                return controller.CheckIsLocked();                
            }
        }
    }
}