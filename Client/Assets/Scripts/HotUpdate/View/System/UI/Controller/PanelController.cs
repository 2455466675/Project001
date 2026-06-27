using MVC;
using static Codice.CM.WorkspaceServer.WorkspaceTreeDataStore;

namespace GameFramework.View.UI
{
    public interface IPanelController
    {
        string AssetPath { get; }
        GroupType PanelGroup { get; }

        bool CheckIsLocked();
        void Show(UIPanel panel, object content);
        void Hide();
        void Refocus();
        void OutFocus();
    }

    public abstract class PanelController : Controller, IPanelController
    {
        private UIPanel panel;
        private object content;

        string IPanelController.AssetPath => this.AssetPath;

        GroupType IPanelController.PanelGroup => this.PanelGroup;

        bool IPanelController.CheckIsLocked()
        {
            return CheckIsLocked();
        }

        void IPanelController.Show(UIPanel panel, object content)
        {
            this.panel = panel;
            this.content = content;
            this.panel.Show();
            OnShow();
        }

        void IPanelController.Hide()
        {
            OnHide();
            ClearBinding();
            this.panel.Hide();
            this.panel = null;
            this.content = null;
        }

        void IPanelController.Refocus() 
        {
            OnRefocus();
        }

        void IPanelController.OutFocus()
        {
            OnOutFocus();
        }

        protected abstract string AssetPath { get; }
        protected virtual GroupType PanelGroup => GroupType.Normal;

        protected T GetWidget<T>() where T : UIWidget
        {
            return panel.GetWidget<T>();
        }

        protected T GetWidget<T>(string key) where T : UIWidget
        {
            return panel.GetWidget<T>(key);
        }

        protected T GetContent<T>()
        {
            if (content == null)
            {
                return default;
            }
            else
            {
                if (content is T result)
                {
                    return result;
                }
                else
                {
                    return default;
                }
            }
        }

        #region ÉúÃüÖÜÆÚ

        protected virtual void OnShow() { }
        protected virtual void OnHide() { }
        protected virtual void OnRefocus() { }
        protected virtual void OnOutFocus() { }
        #endregion
        protected virtual bool CheckIsLocked()
        {
            return false;
        }
    }
}