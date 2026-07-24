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

        private bool isInit;

        public void Init(PanelDefine id)
        {
            if (isInit)
            {
                return;
            }

            controller = Game.GetSystem<UISystem>().GetPanelController(id);
            if (controller == null)
            {
                MDebug.Error("PanelController is null : ", id.ToString());
                return;
            }

            string assetPath = controller.AssetPath;
            if (string.IsNullOrEmpty(assetPath))
            {
                MDebug.Error("面板资源路径未配置 : ", id.ToString());
                return;
            }

            MDebug.Log($"加载面板:{id}， {assetPath}");

            Transform group = GameRoot.GetNode<UINode>().GetGroup(controller.PanelGroup);
            var go = Game.Assets.Instantiate(assetPath, group);
            panel = go.GetComponent<UIPanel>();

            var views = panel.GetNavigationViews();
            var defines = Game.GetSystem<UISystem>().GetNavigationDefines(id);

            int length = GameMath.Min(views.Length, defines.Length);
            navigationListEntities = new Dictionary<NavigationDefine, NavigationListEntity>(length);

            for (int i = 0; i < length; i++)
            {
                NavigationListEntity navigationList = new NavigationListEntity();
                navigationList.Init(views[i], defines[i]);
                navigationListEntities.Add(defines[i], navigationList);
            }
            isInit = true;
        }

        public void Destroy()
        {
            foreach (var item in navigationListEntities)
            {
                item.Value.Destroy();
            }
            navigationListEntities.Clear();

            Game.Assets.ReleaseAsset(panel.gameObject);

            panel = null;
            controller = null;
            isInit = false;
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
                item.Value.Show(content);
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
