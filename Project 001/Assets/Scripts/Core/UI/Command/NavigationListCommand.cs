namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class NavigationListCommand : INavigationCommand
    {
        public bool IsUndoable => Proxy.IsUndoable();

        public ListProxy Proxy { get; private set; }

        private int[] indexs;
        public NavigationListCommand(ListProxy proxy, int[] indexs)
        {
            Proxy = proxy;
            this.indexs = indexs;
        }

        public void OnPop()
        {
            Proxy.Exit();
        }

        public bool OnPush()
        {
            int[] args;
            if (indexs != null)
            {
                args = indexs;
            }
            else
            {
                args = new int[1] { 0 };
            }
            return Proxy.InFocus(args);
        }

        public bool OnRise()
        {
            if (Proxy.Refocus())
            {
                return true;
            }
            else
            {
                MLog.Error($"ÁÐ±í¾Û½¹Ê§°Ü:{Proxy.Name}");
                return false;
            }         
        }

        public bool OnSink()
        {
            if (Proxy.OutFocus())
            {
                return true;
            }
            else
            {
                MLog.Error($"ÁÐ±íÊ§½¹Ê§°Ü:{Proxy.Name}");
                return false;
            }
        }
    }
}

