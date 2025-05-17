namespace Game.UI.Input
{
    public enum ModuleType 
    {
        Undefined = 0,
        Basal     = 1,
        Panel     = 2,
        Battle    = 3,
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class InputModule : InputCammand
    {
        public abstract ModuleType ModuleType { get;}

        public virtual void Navigate(NavigationListDefine list_ID, int[] navigateIndexs, object intent) 
        {
            NavigationGroupDefine groupDefine = Game.UI.ListDefineToGroupDefine(list_ID);
            if (groupDefine == NavigationGroupDefine.Undefined)
            {
                return;
            }

            NavigationListCammand listCammand = new NavigationListCammand(list_ID, navigateIndexs);
            NavigationGroupCammand groupCammand;

            if (TryPeek(out InputCammand cammand))
            {
                groupCammand = cammand as NavigationGroupCammand;
                if (groupCammand != null && groupCammand.Define == groupDefine)
                {
                    if (groupCammand.TryPeek(out InputCammand sub))
                    {
                        if (sub is NavigationListCammand _sub && _sub.Define == list_ID)
                        {
                            return;
                        }
                    }
                    groupCammand.Push(listCammand);
                    return;
                }
            }

            groupCammand = new NavigationGroupCammand(groupDefine, intent);
            Push(groupCammand);
            groupCammand.Push(listCammand);
        }
    }
}
