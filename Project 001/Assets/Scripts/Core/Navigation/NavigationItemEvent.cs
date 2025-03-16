using System;
using UnityEngine;
using UnityEngine.Events;

namespace Navigation
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationItemEvent : MonoBehaviour
    {
        [Serializable]
        public class NavigationItemUnityEvent : UnityEvent<NavigationItem>
        {
        }

        [SerializeField]
        private NavigationItemUnityEvent onSubmitEvent;
        [SerializeField]
        private NavigationItemUnityEvent onSelectEvent;
        [SerializeField]
        private NavigationItemUnityEvent onDeselectEvent;
        [SerializeField]
        private NavigationItemUnityEvent onOutFocusEvent;
        [SerializeField]
        private NavigationItemUnityEvent onMoveUpEvent;
        [SerializeField]
        private NavigationItemUnityEvent onMoveDownEvent;
        [SerializeField]
        private NavigationItemUnityEvent onMoveLeftEvent;
        [SerializeField]
        private NavigationItemUnityEvent onMoveRightEvent;

        public void OnSubmit(NavigationItem item)
        {
            onSubmitEvent?.Invoke(item);
        }

        public void OnSelect(NavigationItem item)
        {
            onSelectEvent?.Invoke(item);
        }

        public void OnDeselect(NavigationItem item)
        {
            onDeselectEvent?.Invoke(item);
        }

        public void OnOutFocus(NavigationItem item)
        {
            onOutFocusEvent?.Invoke(item);
        }

        public void OnMoveUp(NavigationItem item)
        {
            onMoveUpEvent?.Invoke(item);
        }

        public void OnMoveDown(NavigationItem item)
        {
            onMoveDownEvent?.Invoke(item);
        }

        public void OnMoveLeft(NavigationItem item)
        {
            onMoveLeftEvent?.Invoke(item);
        }

        public void OnMoveRight(NavigationItem item)
        {
            onMoveRightEvent?.Invoke(item);
        }
    }
}
