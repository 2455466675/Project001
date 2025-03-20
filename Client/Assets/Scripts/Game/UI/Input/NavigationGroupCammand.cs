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
            var entity = GetEntity();
            entity?.Refocus();
            return true;
        }

        protected override void OnSink()
        {
            var entity = GetEntity();
            entity?.OutFocus();            
        }

        private NavigationGroupEntity GetEntity() 
        {
            return GameWorld.Root.GetComponent<UIComponent>().GetNavigationGroupEntity(Define);
        }
    }
}
