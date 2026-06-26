using Config;
using GameFramework.Core;
using GameFramework.Utility;
using GameFramework.Utility.GameDefine;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.View.UI
{
    public class PanelEntity
    {
        public bool IsShowing { get; private set; }

        private UIPanel panel;
        private IPanelController controller;
        private Dictionary<NavigationDefine, NavigationListEntity> navigationListEntities;

        public void Init(PanelDefine id)
        {

            controller = Game.GetSystem<UISystem>().GetPanelController(id);
            if (controller == null)
            {
                MDebug.Error("PanelController is null : ", id.ToString());
                return;
            }

            string assetPath;
            Transform group;

            if (string.IsNullOrEmpty(controller.AssetPath))
            {
                var cfg = Game.Config.Find<PanelCfg>((int)id);  //废弃
                assetPath = cfg.Path;
                group = GameRoot.GetNode<UINode>().GetGroup(cfg.GroupType);
            }
            else
            {
                assetPath = controller.AssetPath;
                group = GameRoot.GetNode<UINode>().GetGroup(controller.PanelGroup);
            }

            var go = Game.Assets.Instantiate(assetPath, group);
            panel = go.GetComponent<UIPanel>();

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
            if (IsShowing)
            {
                return;
            }
            IsShowing = true;
            controller?.Show(panel, content);
            foreach (var item in navigationListEntities)
            {
                item.Value.Show();
            }
        }

        public void Hide()
        {
            if (!IsShowing)
            {
                return;
            }
            IsShowing = false;

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