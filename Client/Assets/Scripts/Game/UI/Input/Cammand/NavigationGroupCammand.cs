using Game.UI.Input;

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
            NavigationGroupEntity entity = GetEntity();
            entity?.Hide();
            return;
        }

        protected override bool OnPush()
        {
            NavigationGroupEntity entity = GetEntity();
            entity?.Show();
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
            return Game.UI.GetNavigationGroupEntity(Define);
        }
    }
}
