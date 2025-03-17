namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationGroupCammand : InputCammand
    {
        public NavigationGroupDefine Define { get; private set; }

        public NavigationGroupCammand(NavigationGroupDefine define)
        {
            Define = define;
        }

        protected override void OnPop()
        {
            GameWorld.Root.GetComponent<UIComponent>().HideGroup(Define);
        }

        protected override bool OnPush()
        {
            GameWorld.Root.GetComponent<UIComponent>().ShowGroup(Define);
            return true;
        }

        protected override bool OnRise()
        {
            GameWorld.Root.GetComponent<UIComponent>().RefocusGroup(Define);
            return true;
        }

        protected override void OnSink()
        {
            GameWorld.Root.GetComponent<UIComponent>().OutFocusGroup(Define);
        }
    }
}
