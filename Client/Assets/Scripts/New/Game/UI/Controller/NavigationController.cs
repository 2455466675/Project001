using System.Collections;
using System.Collections.Generic;

namespace GameFramework.UI
{
    public abstract class NavigationController
    {
        private NavigationView m_View;

        public void Show(NavigationView view)
        {
            m_View = view;
            Register();
            OnShow();
        }

        public void Hide() 
        {
            OnHide();
            Unregister();
            m_View = null;
        }

        #region

        public void Move(float h, float v)
        {
            if (m_View == null)
            {
                return;
            }
            m_View.Move(h, v);
        }

        public void Submit()
        {
            if (m_View == null)
            {
                return;
            }
            m_View.Submit();
        }

        public bool InFocus(bool isRefocus, int[] indexs = null)
        {
            if (m_View == null)
            {
                return false;
            }
            return m_View.InFocus(isRefocus, indexs);
        }

        public void OutFocus()
        {
            if (m_View == null)
            {
                return;
            }
            m_View.OutFocus();
        }

        public void Exit()
        {
            if (m_View == null)
            {
                return;
            }
            m_View.Exit();
        }
        #endregion

        protected virtual void OnShow() { }
        protected virtual void OnHide() { }

        #region

        protected virtual void OnBindData(NavigationItemView itemView, int dataIndex)
        {
            MDebug.Log("OnBindData", dataIndex);
        }

        protected virtual void OnUnbindData(NavigationItemView itemView, int dataIndex)
        {
            MDebug.Log("OnUnbindData", dataIndex);
        }

        protected virtual void OnSelect(NavigationItemView itemView, int dataIndex)
        {
            MDebug.Log("OnSelect", dataIndex);
        }

        protected virtual void OnDeselect(NavigationItemView itemView, int dataIndex)
        {
            MDebug.Log("OnDeselect", dataIndex);
        }

        protected virtual void OnSubmit(NavigationItemView itemView, int dataIndex)
        {
            MDebug.Log("OnSubmit", dataIndex);
        }

        protected virtual void OnMoveUp(NavigationItemView itemView, int dataIndex)
        {
            MDebug.Log("OnMoveUp", dataIndex);
        }

        protected virtual void OnMoveDown(NavigationItemView itemView, int dataIndex)
        {
            MDebug.Log("OnMoveDown", dataIndex);
        }

        protected virtual void OnMoveLeft(NavigationItemView itemView, int dataIndex)
        {
            MDebug.Log("OnMoveLeft", dataIndex);
        }

        protected virtual void OnMoveRight(NavigationItemView itemView, int dataIndex)
        {
            MDebug.Log("OnMoveRight", dataIndex);
        }
        #endregion

        private void Register() 
        {
            if (m_View == null) 
            {
                return;
            }

            m_View.OnSelectItem += OnSelect;
            m_View.OnDeselectItem += OnDeselect;
            m_View.OnSubmitItem += OnSubmit;
            m_View.OnMoveUpItem += OnMoveUp;
            m_View.OnMoveDownItem += OnMoveDown;
            m_View.OnMoveLeftItem += OnMoveLeft;
            m_View.OnMoveRigthItem += OnMoveRight;
            m_View.OnItemBindData += OnBindData;
            m_View.OnItemUnbindData += OnUnbindData;
        }

        private void Unregister()
        {
            if (m_View == null)
            {
                return;
            }

            m_View.OnSelectItem -= OnSelect;
            m_View.OnDeselectItem -= OnDeselect;
            m_View.OnSubmitItem -= OnSubmit;
            m_View.OnMoveUpItem -= OnMoveUp;
            m_View.OnMoveDownItem -= OnMoveDown;
            m_View.OnMoveLeftItem -= OnMoveLeft;
            m_View.OnMoveRigthItem -= OnMoveRight;
            m_View.OnItemBindData -= OnBindData;
            m_View.OnItemUnbindData -= OnUnbindData;
        }
    }
}
