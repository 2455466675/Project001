using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Navigation;
using System;

namespace GameFramework.View.UI
{
    public class NavigationView : UIView
    {
        [SerializeField]
        private NavigationList m_List;

        public event Action<NavigationItemView, int> OnItemBindData;
        public event Action<NavigationItemView, int> OnItemUnbindData;
        public event Action<NavigationItemView, int> OnSelectItem;
        public event Action<NavigationItemView, int> OnDeselectItem;
        public event Action<NavigationItemView, int> OnSubmitItem;
        public event Action<NavigationItemView, int> OnMoveUpItem;
        public event Action<NavigationItemView, int> OnMoveDownItem;
        public event Action<NavigationItemView, int> OnMoveLeftItem;
        public event Action<NavigationItemView, int> OnMoveRigthItem;
        public event Func<int, bool> OnCheckItemValid;

        private void Awake()
        {
            Register();
        }

        private void OnDestroy()
        {
            UnRegister();
        }

        private void Register() 
        {
            if (m_List == null)
            {
                return;
            }
            m_List.Init();
            m_List.OnSelectedItem += List_OnSelectedItem;
            m_List.OnDeselectedItem += List_OnDeselectedItem;
            m_List.OnSubmitItem += List_OnSubmitItem;
            m_List.OnMoveItem += List_OnMoveItem;
            m_List.OnItemBindData += List_OnItemBindData;
            m_List.OnItemUnbindData += List_OnItemUnbindData;
            m_List.OnCheckItemValid += List_CheckItemValid;
        }

        private void UnRegister()
        {
            if (m_List == null)
            {
                return;
            }

            m_List.OnSelectedItem -= List_OnSelectedItem;
            m_List.OnDeselectedItem -= List_OnDeselectedItem;
            m_List.OnSubmitItem -= List_OnSubmitItem;
            m_List.OnMoveItem -= List_OnMoveItem;
            m_List.OnItemBindData -= List_OnItemBindData;
            m_List.OnItemUnbindData -= List_OnItemUnbindData;
            m_List.OnCheckItemValid -= List_CheckItemValid;
        }

        public void UpdateDataCount(int count) 
        {
            if(m_List == null) 
            {
                return;
            }
            m_List.UpdateDataCount(count);
        }

        #region

        private void List_OnItemUnbindData(NavigationItem arg)
        {
            if (arg == null) 
            {
                return;
            }

            if (arg.TryGetComponent<NavigationItemView>(out var itemView)) 
            {
                OnItemUnbindData?.Invoke(itemView, arg.IndexOfData);
            }
        }

        private void List_OnItemBindData(NavigationItem arg)
        {
            if (arg == null)
            {
                return;
            }

            if (arg.TryGetComponent<NavigationItemView>(out var itemView))
            {
                OnItemBindData?.Invoke(itemView, arg.IndexOfData);
            }
        }

        private void List_OnMoveItem(float h, float v, NavigationItem arg)
        {
            if (arg == null)
            {
                return;
            }

            if (arg.TryGetComponent<NavigationItemView>(out var itemView))
            {
                int index = arg.IndexOfData;
                if (h > 0)
                {
                    OnMoveRigthItem?.Invoke(itemView, index);
                }
                if (h < 0)
                {
                    OnMoveLeftItem?.Invoke(itemView, index);
                }
                if (v > 0)
                {
                    OnMoveUpItem?.Invoke(itemView, index);
                }
                if (v < 0)
                {
                    OnMoveDownItem?.Invoke(itemView, index);
                }
            }
        }

        private void List_OnSubmitItem(NavigationItem arg)
        {
            if (arg == null)
            {
                return;
            }

            if (arg.TryGetComponent<NavigationItemView>(out var itemView))
            {
                OnSubmitItem?.Invoke(itemView, arg.IndexOfData);
            }
        }

        private void List_OnDeselectedItem(NavigationItem arg)
        {
            if (arg == null)
            {
                return;
            }

            if (arg.TryGetComponent<NavigationItemView>(out var itemView))
            {
                OnDeselectItem?.Invoke(itemView, arg.IndexOfData);
            }
        }

        private void List_OnSelectedItem(NavigationItem arg)
        {
            if (arg == null)
            {
                return;
            }

            if (arg.TryGetComponent<NavigationItemView>(out var itemView))
            {
                OnSelectItem?.Invoke(itemView, arg.IndexOfData);
            }
        }

        private bool List_CheckItemValid(int index) 
        {
            if (OnCheckItemValid == null) 
            {
                return true;
            }
            return OnCheckItemValid.Invoke(index);
        }

        #endregion

        #region

        public void Move(float h, float v)
        {
            if (m_List != null)
            {
                m_List.Move(h, v);
            }
        }

        public void Submit()
        {
            if (m_List != null)
            {
                m_List.Submit();
            }
        }

        public bool InFocus(bool isRefocus, int[] indexs = null)
        {
            if (m_List != null)
            {
                return m_List.InFocus(isRefocus, indexs);
            }
            else
            {
                return false;
            }
        }

        public void OutFocus()
        {
            if (m_List != null)
            {
                m_List.OutFocus();
            }
        }

        public void Exit()
        {
            if (m_List != null)
            {
                m_List.Exit();
            }
        }

        #endregion
    }
}