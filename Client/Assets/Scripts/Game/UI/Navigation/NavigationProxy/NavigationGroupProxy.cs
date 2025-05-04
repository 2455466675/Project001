using Config;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationGroupProxy
    {
        public NavigationListView[] Children
        {
            get
            {
                if (groupView == null)

                {
                    return new NavigationListView[0];
                }
                else
                {
                    return groupView.Children;
                }
            }
        }

        protected NavigationGroupView groupView;

        public virtual void LoadGroup(NavigationGroupDefine define) 
        {
            var cfg = Game.Config.Find<PanelCfg>((int)define);
            if (cfg == null ) 
            {
                return;
            }

            var parent = Game.UI.Root.GetGroupContainer(cfg.GroupType);
            if (parent == null) 
            {
                return;
            }

            var go = Game.Resource.LoadAndInstantiate(cfg.Path, parent);
            groupView = go.GetComponent<NavigationGroupView>();
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
