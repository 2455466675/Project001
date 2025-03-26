using Config;
using Navigation;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationGroupProxy
    {
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
            return go.GetComponent<NavigationGroupView>();
        }

        public virtual void InFocus(NavigationGroupView group, bool isRefocus)
        {
            if (group == null) 
            {
                return;
            }

            if (isRefocus) 
            {
                if (group.TryGetComponent<CanvasGroup>(out var canvasGroup)) 
                {
                    canvasGroup.alpha = 1;
                }
            }
            else
            {
                group.gameObject.SetActive(true);
                group.transform.SetAsLastSibling();                
            }
        }

        public virtual void OutFocus(NavigationGroupView group)
        {
            if (group == null)
            {
                return;
            }
            if (group.TryGetComponent<CanvasGroup>(out var canvasGroup))
            {
                canvasGroup.alpha = 0.6f;
            }
        }

        public virtual void Exit(NavigationGroupView group)
        {
            if (group == null)
            {
                return;
            }
            group.gameObject.SetActive(false);
            group.transform.SetAsLastSibling();
        }
    }
}
