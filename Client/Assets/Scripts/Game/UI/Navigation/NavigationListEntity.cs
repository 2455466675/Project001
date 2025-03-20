using Navigation;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationListEntity : ECS.Entity
    {
        public NavigationListDefine ListDefine { get; private set; }

        public bool IsLocked => proxy != null && proxy.IsLocked();

        private NavigationListProxy proxy;

        private NavigationList list;

        public void Init(NavigationListDefine define, NavigationList list) 
        {
            proxy = Parent.Parent.GetComponent<NavigationProxy>().GetNavigationListProxy(define);
            this.ListDefine = define;
            this.list = list;

            //list.Init();
            //(list as FluidNavigationList).UpdateItemCount(5);
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
