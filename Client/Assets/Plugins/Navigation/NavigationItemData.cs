using System.Collections.Generic;

namespace Navigation
{
    public class NavigationItemData : INavigationItemData
    {
        private readonly HashSet<IRefreshable> objs = new HashSet<IRefreshable>();

        public void Bind(IRefreshable obj)
        {
            this.objs.Add(obj);
        }

        public void Unbind(IRefreshable obj)
        {
            this.objs.Remove(obj);
        }

        public void Refresh() 
        {
            foreach (var obj in objs)
            {
                if (obj != null) 
                {
                    obj.Refresh();
                }
            }
        }
    }
}