using Game.Core;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace Game.UI
{
    [Serializable]
    public class OnSubmitEvent : UnityEvent<UINotification>
    {       
    }

    [Serializable]
    public class OnSelectEvent : UnityEvent<UINotification>
    {
    }

    [Serializable]
    public class OnDeselectEvent : UnityEvent<UINotification>
    {
    }

    [Serializable]
    public class OnMoveUpEvent : UnityEvent<UINotification>
    {
    }

    [Serializable]
    public class OnMoveDownEvent : UnityEvent<UINotification>
    {
    }

    [Serializable]
    public class OnMoveLeftEvent : UnityEvent<UINotification>
    {
    }

    [Serializable]
    public class OnMoveRightEvent : UnityEvent<UINotification>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public class UINotify : MonoBehaviour
    {
        [SerializeField]
        private OnSubmitEvent onSubmitEvent;
        [SerializeField]
        private OnSubmitEvent onSelectEvent;
        [SerializeField]
        private OnSubmitEvent onDeselectEvent;
        [SerializeField]
        private OnSubmitEvent onMoveUpEvent;
        [SerializeField]
        private OnSubmitEvent onMoveDownEvent;
        [SerializeField]
        private OnSubmitEvent onMoveLeftEvent;
        [SerializeField]
        private OnSubmitEvent onMoveRightEvent;

        public void OnSubmit(GuidableItemBase listItem)
        {
            onSubmitEvent?.Invoke(new UINotification(listItem));
        }

        public void OnSelect(GuidableItemBase listItem)
        {
            onSelectEvent?.Invoke(new UINotification(listItem));
        }

        public void OnDeselect(GuidableItemBase listItem)
        {
            onDeselectEvent?.Invoke(new UINotification(listItem));
        }

        public void OnMoveUp(GuidableItemBase listItem)
        {
            onMoveUpEvent?.Invoke(new UINotification(listItem));
        }

        public void OnMoveDown(GuidableItemBase listItem)
        {
            onMoveDownEvent?.Invoke(new UINotification(listItem));
        }

        public void OnMoveLeft(GuidableItemBase listItem)
        {
            onMoveLeftEvent?.Invoke(new UINotification(listItem));  
        }

        public void OnMoveRight(GuidableItemBase listItem)
        {
            onMoveRightEvent?.Invoke(new UINotification(listItem));
        }     
    }
}