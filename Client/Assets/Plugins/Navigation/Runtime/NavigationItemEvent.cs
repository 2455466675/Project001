using System;
using UnityEngine;
using UnityEngine.Events;

namespace Navigation
{
    /// <summary>
    /// 导航元素事件转发组件：把选中/取消/失焦/提交/移动等事件通过 UnityEvent 暴露到 Inspector，
    /// 便于在编辑器中无代码挂接回调。
    /// </summary>
    public class NavigationItemEvent : MonoBehaviour
    {
        [Serializable]
        public class NavigationItemUnityEvent : UnityEvent<NavigationItem>
        {
        }

        [SerializeField]
        private NavigationItemUnityEvent m_OnSubmit;
        [SerializeField]
        private NavigationItemUnityEvent m_OnSelect;
        [SerializeField]
        private NavigationItemUnityEvent m_OnDeselect;
        [SerializeField]
        private NavigationItemUnityEvent m_OnOutFocus;
        [SerializeField]
        private NavigationItemUnityEvent m_OnMoveUp;
        [SerializeField]
        private NavigationItemUnityEvent m_OnMoveDown;
        [SerializeField]
        private NavigationItemUnityEvent m_OnMoveLeft;
        [SerializeField]
        private NavigationItemUnityEvent m_OnMoveRight;

        public void OnSubmit(NavigationItem item)
        {
            m_OnSubmit?.Invoke(item);
        }

        public void OnSelect(NavigationItem item)
        {
            m_OnSelect?.Invoke(item);
        }

        public void OnDeselect(NavigationItem item)
        {
            m_OnDeselect?.Invoke(item);
        }

        public void OnOutFocus(NavigationItem item)
        {
            m_OnOutFocus?.Invoke(item);
        }

        public void OnMoveUp(NavigationItem item)
        {
            m_OnMoveUp?.Invoke(item);
        }

        public void OnMoveDown(NavigationItem item)
        {
            m_OnMoveDown?.Invoke(item);
        }

        public void OnMoveLeft(NavigationItem item)
        {
            m_OnMoveLeft?.Invoke(item);
        }

        public void OnMoveRight(NavigationItem item)
        {
            m_OnMoveRight?.Invoke(item);
        }
    }
}
