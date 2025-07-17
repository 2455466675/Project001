using Game.UI.Input;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationGroupCammand : InputCammand
    {
        public NavigationGroupDefine Define { get; private set; }

        private object intent;

        public NavigationGroupCammand(NavigationGroupDefine define, object intent = null)
        {
            Define = define;
            this.intent = intent;
        }

        protected override void OnPop(bool isPopAll = false)
        {
            var entity = GetEntity();
            entity?.Hide();
        }

        protected override bool OnPush()
        {
            var entity = GetEntity();
            entity?.Show(intent);
            return true;
        }

        protected override bool OnRise(bool isPopAll = false)
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

        protected override bool CheckIsLocked()
        {
            var entity = GetEntity();
            return entity.IsLocked;
        }
    }
}
