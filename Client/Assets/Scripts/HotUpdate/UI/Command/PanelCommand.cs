using GameFramework.Core;

namespace GameFramework.UI 
{
    public class PanelCommand : InputCommand
    {
        public PanelDefine Define { get; private set; }
        private object content;

        public PanelCommand(PanelDefine define, object content = null)
        {
            Define = define;
            this.content = content;
        }

        protected override void OnPop()
        {
            Game.GetModule<UIManager>().HidePanel(Define);
        }

        protected override bool OnPush()
        {
            Game.GetModule<UIManager>().ShowPanel(Define, content);
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

        protected override void OnInputAction(InputContext context)
        {
            //TODO 界面输入事件
        }

        private PanelComponent GetPanel()
        {
            var entity = Game.GetModule<UIManager>().GetPanelEntity(Define);
            return entity.GetComponent<PanelComponent>();
        }
    }
}