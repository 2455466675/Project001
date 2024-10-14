namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class NavigationListCommand : INavigationCommand
    {
        public bool IsUndoable => throw new global::System.NotImplementedException();

        public ListProxy Proxy { get; private set; }

        public NavigationListCommand(ListProxy proxy)
        {
            Proxy = proxy;
        }

        public void OnPop()
        {
            Proxy.Close();
        }

        public bool OnPush()
        {
            return Proxy.InFocus(0);
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

