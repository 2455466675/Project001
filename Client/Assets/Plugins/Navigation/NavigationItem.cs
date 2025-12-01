using System;
using UnityEngine;

namespace Navigation
{
    public class NavigationItem : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField]
        private int m_IndexOfList = -1;
        public int IndexOfList => m_IndexOfList;

        private int m_IndexOfData = -1;
        public int IndexOfData => m_IndexOfData;

        internal bool IsValid 
        {
            get 
            {
                if (DataValidChecker == null) 
                {
                    return true;
                }
                else
                {
                    return DataValidChecker.Invoke(IndexOfData);
                }
            }
        }
        internal event Func<int, bool> DataValidChecker;

        [SerializeField]
        private NavigationItemSelector m_Selector;
        [SerializeField]
        private NavigationItemEvent m_Event;

        #region

        internal void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        internal void SetListIndex(int index)
        {
            m_IndexOfList = index;
        }

        internal void SetDataIndex(int index) 
        {
            m_IndexOfData = index;
        }

        #endregion

        #region Event

        internal void OnSelect()
        {
            if (m_Selector != null)
            {
                m_Selector.OnSelect();
            }
            if (m_Event != null)
            {
                m_Event.OnSelect(this);
            }
        }

        internal void OnDeselect()
        {
            if (m_Selector != null)
            {
                m_Selector.OnDeselect();
            }
            if (m_Event != null)
            {
                m_Event.OnDeselect(this);
            }
        }

        internal void OutFocus()
        {
            if (m_Selector != null)
            {
                m_Selector.OnOutFocus();
            }
            if (m_Event != null)
            {
                m_Event.OnOutFocus(this);
            }
        }

        internal void OnSubmit()
        {
            if (m_Event != null)
            {
                m_Event.OnSubmit(this);
            }
        }

        internal void OnMoveUp()
        {
            if (m_Event != null)
            {
                m_Event.OnMoveUp(this);
            }
        }

        internal void OnMoveDown()
        {
            if (m_Event != null)
            {
                m_Event.OnMoveDown(this);
            }
        }

        internal void OnMoveLeft()
        {
            if (m_Event != null)
            {
                m_Event.OnMoveLeft(this);
            }
        }

        internal void OnMoveRight()
        {
            if (m_Event != null)
            {
                m_Event.OnMoveRight(this);
            }
        }
        #endregion
    }
}