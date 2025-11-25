using System;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework.UI
{
    public abstract class NavigationController
    {
        private class DataBinder
        {
            private NavigationItemView m_ItemView;
            private DataModel m_DataModel;
            private Action<NavigationItemView, DataModel> m_Action;

            public void Bind(NavigationItemView view, DataModel dataModel, Action<NavigationItemView, DataModel> action)
            {
                m_ItemView = view;
                m_DataModel = dataModel;
                m_Action = action;
                m_DataModel.OnValueChanged += DataModel_OnValueChanged;

                DataModel_OnValueChanged(string.Empty);
            }

            public void Unbind()
            {
                m_DataModel.OnValueChanged -= DataModel_OnValueChanged;
                m_ItemView = null;
                m_DataModel = null;
                m_Action = null;
            }

            private void DataModel_OnValueChanged(string key)
            {
                m_Action?.Invoke(m_ItemView, m_DataModel);
            }
        }

        private NavigationView m_View;
        private IReadOnlyList<DataModel> m_Datas;
        private Dictionary<int, DataBinder> m_Binders = new Dictionary<int, DataBinder>();

        public bool IsLocked => CheckIsLocked();

        protected void SetData(IReadOnlyList<DataModel> datas)
        {
            if (m_View == null)
            {
                return;
            }
            datas ??= new List<DataModel>();
            m_Datas = datas;
            m_View.UpdateDataCount(datas.Count);
        }

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
            ClearBinders();
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

        public bool InFocus(bool isRefocus, int[] indexs)
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

        #region

        private bool CheckDataValid(int dataIndex)
        {
            bool result = m_Datas != null && dataIndex >= 0 && dataIndex < m_Datas.Count;
            if (!result)
            {
                MDebug.Error("数据无效 : ", dataIndex);
            }
            return result;
        }

        private void OnBindData(NavigationItemView itemView, int dataIndex)
        {
            if (!CheckDataValid(dataIndex))
            {
                return;
            }

            DataModel dataModel = m_Datas[dataIndex];
            DataBinder binder = new DataBinder();
            BindItemView(itemView, dataModel);
            binder.Bind(itemView, dataModel, RefreshItemView);
            m_Binders[dataIndex] = binder;
        }

        private void OnUnbindData(NavigationItemView itemView, int dataIndex)
        {
            if (m_Binders.TryGetValue(dataIndex, out DataBinder binder))
            {
                binder.Unbind();
                UnbindItemView(itemView, m_Datas[dataIndex]);
                m_Binders.Remove(dataIndex);
            }
        }

        private void OnSelect(NavigationItemView itemView, int dataIndex)
        {
            if (!CheckDataValid(dataIndex))
            {
                return;
            }

            DataModel dataModel = m_Datas[dataIndex];
            SelectItemView(itemView, dataModel);
        }

        private void OnDeselect(NavigationItemView itemView, int dataIndex)
        {
            if (!CheckDataValid(dataIndex))
            {
                return;
            }

            DataModel dataModel = m_Datas[dataIndex];
            DeselectItemView(itemView, dataModel);
        }

        private void OnSubmit(NavigationItemView itemView, int dataIndex)
        {
            if (!CheckDataValid(dataIndex))
            {
                return;
            }

            DataModel dataModel = m_Datas[dataIndex];
            SubmitItemView(itemView, dataModel);
        }

        private void OnMoveUp(NavigationItemView itemView, int dataIndex)
        {
            if (!CheckDataValid(dataIndex))
            {
                return;
            }

            DataModel dataModel = m_Datas[dataIndex];
            MoveUpItemView(itemView, dataModel);
        }

        private void OnMoveDown(NavigationItemView itemView, int dataIndex)
        {
            if (!CheckDataValid(dataIndex))
            {
                return;
            }

            DataModel dataModel = m_Datas[dataIndex];
            MoveDownItemView(itemView, dataModel);
        }

        private void OnMoveLeft(NavigationItemView itemView, int dataIndex)
        {
            if (!CheckDataValid(dataIndex))
            {
                return;
            }

            DataModel dataModel = m_Datas[dataIndex];
            MoveLeftItemView(itemView, dataModel);
        }

        private void OnMoveRight(NavigationItemView itemView, int dataIndex)
        {
            if (!CheckDataValid(dataIndex))
            {
                return;
            }

            DataModel dataModel = m_Datas[dataIndex];
            MoveRightItemView(itemView, dataModel);
        }

        private bool CheckValid(int dataIndex)
        {
            if (!CheckDataValid(dataIndex))
            {
                return false;
            }
            DataModel dataModel = m_Datas[dataIndex];
            return CheckItemIsValid(dataModel);
        }

        #endregion

        #region

        protected virtual void OnShow() { }
        protected virtual void OnHide() { }

        protected virtual void BindItemView(NavigationItemView itemView, DataModel dataModel) { }
        protected virtual void UnbindItemView(NavigationItemView itemView, DataModel dataModel) { }
        protected virtual void RefreshItemView(NavigationItemView itemView, IReadOnlyDataModel dataModel) { }
        protected virtual void SelectItemView(NavigationItemView itemView, DataModel dataModel) { }
        protected virtual void DeselectItemView(NavigationItemView itemView, DataModel dataModel) { }
        protected virtual void SubmitItemView(NavigationItemView itemView, DataModel dataModel) { }
        protected virtual void MoveUpItemView(NavigationItemView itemView, DataModel dataModel) { }
        protected virtual void MoveDownItemView(NavigationItemView itemView, DataModel dataModel) { }
        protected virtual void MoveLeftItemView(NavigationItemView itemView, DataModel dataModel) { }
        protected virtual void MoveRightItemView(NavigationItemView itemView, DataModel dataModel) { }

        protected virtual bool CheckItemIsValid(DataModel dataModel) 
        {
            return true;
        }

        protected virtual bool CheckIsLocked() 
        {
            return false;
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
            m_View.OnCheckItemValid += CheckValid;
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
            m_View.OnCheckItemValid -= CheckValid;
        }

        private void ClearBinders() 
        {
            foreach (var item in m_Binders)
            {
                item.Value.Unbind();
            }
            m_Binders.Clear();
        }
    }
}
