using Config;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationGroupProxy
    {
        protected NavigationGroupView groupView;

        public NavigationGroupView LoadGroup(NavigationGroupDefine define) 
        {
            var cfg = Game.Config.Find<PanelCfg>((int)define);
            if (cfg == null ) 
            {
                return null;
            }

            var parent = Game.UI.Root.GetGroupContainer(cfg.GroupType);
            if (parent == null) 
            {
                return null;
            }

            var go = Game.Resource.LoadAndInstantiate(cfg.Path, parent);
            groupView = go.GetComponent<NavigationGroupView>();
            return groupView;
        }

        public virtual void Show() 
        {
            if (groupView == null)
            {
                return;
            }
            groupView.gameObject.SetActive(true);
            groupView.transform.SetAsLastSibling();
        }

        public virtual void Hide() 
        {
            if (groupView == null)
            {
                return;
            }
            groupView.gameObject.SetActive(false);
            groupView.transform.SetAsLastSibling();
        }

        public virtual void OutFocus() 
        {
            if (groupView == null)
            {
                return;
            }
            if (groupView.TryGetComponent<CanvasGroup>(out var canvasGroup))
            {
                canvasGroup.alpha = 0.6f;
            }
        }

        public virtual void Refocus() 
        {
            if (groupView == null)
            {
                return;
            }
            if (groupView.TryGetComponent<CanvasGroup>(out var canvasGroup))
            {
                canvasGroup.alpha = 1;
            }
        }

        public void OnDestroy() 
        {
            UnityEngine.Object.Destroy(groupView);
            groupView = null;
        }
    }
}
