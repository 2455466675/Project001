using GameFramework.Core;
using GameFramework.Utility.GameDefine;

namespace GameFramework.View.UI
{
    public class PanelCammand : GameInputCammand
    {
        public PanelDefine Define { get; private set; }
        private object content;

        public PanelCammand(PanelDefine define, object content = null)
        {
            Define = define;
            this.content = content;
        }

        protected override void OnPop()
        {
            Game.GetSystem<UISystem>().HidePanel(Define);
        }

        protected override bool OnPush()
        {
            Game.GetSystem<UISystem>().ShowPanel(Define, content);
            return true;
        }

        protected override bool OnRise()
        {
            var panel = GetPanel();
            panel?.Refocus();
            return true;
        }

        protected override void OnSink()
        {
            var panel = GetPanel();
            panel?.OutFocus();
        }

        protected override bool CheckLocked()
        {
            var panel = GetPanel();
            return panel.CheckLocked();
        }

        private PanelEntity GetPanel()
        {
            var entity = Game.GetSystem<UISystem>().GetPanelEntity(Define);
            return entity;
        }
    }
}