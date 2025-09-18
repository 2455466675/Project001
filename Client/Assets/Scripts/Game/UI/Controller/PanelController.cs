namespace GameFramework.UI
{
    public abstract class PanelController
    {
        private UIPanel m_Panel;
        private object m_Content;

        public bool IsLocked => CheckIsLocked();

        public void Show(UIPanel panel, object content)
        {
            m_Panel = panel;
            m_Content = content;

            m_Panel.Show();
            OnShow();
        }

        public void Hide()
        {
            OnHide();
            m_Panel.Hide();

            m_Panel = null;
            m_Content = null;
        }

        public void Refocus() 
        {
            OnRefocus();
        }

        public void OutFocus()
        {
            OnOutFocus();
        }

        protected T GetWidget<T>() where T : UIWidget
        {
            return m_Panel.GetWidget<T>();
        }

        protected T GetContent<T>() where T : class
        {
            if (m_Content == null)
            {
                return default;
            }
            else
            {
                return m_Content as T;
            }
        }

        #region

        protected virtual void OnShow() { }
        protected virtual void OnHide() { }
        protected virtual void OnRefocus() { }
        protected virtual void OnOutFocus() { }
        protected virtual bool CheckIsLocked()
        {
            return false;
        }

        #endregion
    }
}