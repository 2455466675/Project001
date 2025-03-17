using Navigation;
using System.Collections;
using System.Collections.Generic;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationListEntity : ECS.Entity
    {
        private NavigationListProxy proxy;

        private NavigationList list;

        public void Init(NavigationList list) 
        {
            this.list = list;
            proxy = new NavigationListProxy();

            list.Init();
            (list as FluidNavigationList).UpdateItemCount(5);
        }

        public void Move(float h, float v)
        {
            proxy?.Move(list, h, v);
        }

        public void Submit()
        {
            proxy?.Submit(list);
        }

        public bool InFocus(bool isRefocus, int[] indexs = null)
        {
            if (proxy == null) 
            {
                return false;
            }
            else
            {
                return proxy.InFocus(list, isRefocus, indexs);                
            }
        }

        public void OutFocus()
        {
            proxy?.OutFocus(list);
        }

        public void Exit()
        {
            proxy?.Exit(list);
        }

        protected override void OnDestroy()
        {
            list = null;
            proxy = null;
        }
    }
}
