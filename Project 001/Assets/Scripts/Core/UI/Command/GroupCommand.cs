using Game.Core;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class GroupCommand : INavigationCommand
    {
        public UIDefine.Group_ID GroupID { get; private set; }

        public bool IsUndoable 
        {
            get 
            {
                var group = GetNavigationGroup();
                return group != null && group.IsUndoable;
            }
        }

        private readonly int[] defaultIndexs;
        public GroupCommand(UIDefine.Group_ID groupID, int[] defaultIndexs)
        {
            GroupID = groupID;
            this.defaultIndexs = defaultIndexs;
        }

        public void OnPop()
        {
            //ÍË³ö
            var group = GetNavigationGroup();
            group?.Exit();
        }

        public bool OnPush()
        {
            int[] args;
            if (defaultIndexs != null)
            {
                args = defaultIndexs;
            }
            else
            {
                args = new int[1] { 0 };
            }
            //¾Û½¹
            var group = GetNavigationGroup();
            group?.InFocus(false, args);
            return true;
        }

        public bool OnRise()
        {
            //ÖØÑ¡
            var group = GetNavigationGroup();
            group?.InFocus(true);
            return true;
        }

        public bool OnSink()
        {
            //Ê§½¹
            var group = GetNavigationGroup();
            group?.OutFocus();
            return true;
        }

        private NavigationGroupComponent GetNavigationGroup() 
        {
            return GameWorld.Instance.GetComponent<UIComponent>().GetNavigationGroup(GroupID);
        }
    }
}
