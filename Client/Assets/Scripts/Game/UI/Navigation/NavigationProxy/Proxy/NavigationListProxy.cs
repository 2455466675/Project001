using Navigation;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationListProxy
    {
        public virtual void Move(NavigationList list, float h, float v)
        {
            if (list == null) 
            {
                return;
            }
            list.Move(h, v);
        }

        public virtual void Submit(NavigationList list)
        {
            if (list == null)
            {
                return;
            }
            list.Submit();
        }

        public virtual bool InFocus(NavigationList list, bool isRefocus, int[] indexs = null)
        {
            if (list == null)
            {
                return false;
            }
            return list.InFocus(isRefocus, indexs);
        }

        public virtual void OutFocus(NavigationList list)
        {
            if (list == null)
            {
                return;
            }
            list.OutFocus();
        }

        public virtual void Exit(NavigationList list) 
        {
            if (list == null)
            {
                return;
            }
            list.Exit();
        }
    }
}
