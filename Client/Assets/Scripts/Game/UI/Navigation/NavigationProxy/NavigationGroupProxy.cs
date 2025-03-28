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
            var cfg = GameWorld.Root.GetComponent<ConfigComponent>().Find<NavigationGroupCfg>((int)define);
            if (cfg == null ) 
            {
                return null;
            }

            var parent = GameWorld.Root.GetComponent<UIComponent>().Root.GetGroupContainer(cfg.GroupType);
            if (parent == null) 
            {
                return null;
            }

            var go = GameWorld.Root.GetComponent<ResourceComponent>().LoadAndInstantiate(cfg.Path, parent);
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
