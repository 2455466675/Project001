using System;
using UnityEngine;
using UnityEngine.Events;

namespace Navigation
{
    /// <summary>
    /// 
    /// </summary>
	public sealed class GuidableItemEvent : MonoBehaviour
	{
        [Serializable]
        public class OnSubmitEvent : UnityEvent<GuidableItem>
        {
        }

        [SerializeField]
        private OnSubmitEvent onSubmitEvent;
        [SerializeField]
        private OnSubmitEvent onSelectEvent;
        [SerializeField]
        private OnSubmitEvent onDeselectEvent;
        [SerializeField]
        private OnSubmitEvent onOutFocusEvent;
        [SerializeField]
        private OnSubmitEvent onMoveUpEvent;
        [SerializeField]
        private OnSubmitEvent onMoveDownEvent;
        [SerializeField]
        private OnSubmitEvent onMoveLeftEvent;
        [SerializeField]
        private OnSubmitEvent onMoveRightEvent;

        public void OnSubmit(GuidableItem item)
        {
            onSubmitEvent?.Invoke(item);
        }

        public void OnSelect(GuidableItem item)
        {
            onSelectEvent?.Invoke(item);
        }

        public void OnDeselect(GuidableItem item)
        {
            onDeselectEvent?.Invoke(item);
        }

        public void OnOutFocus(GuidableItem item)
        {
            onOutFocusEvent?.Invoke(item);
        }

        public void OnMoveUp(GuidableItem item)
        {
            onMoveUpEvent?.Invoke(item);
        }

        public void OnMoveDown(GuidableItem item)
        {
            onMoveDownEvent?.Invoke(item);
        }

        public void OnMoveLeft(GuidableItem item)
        {
            onMoveLeftEvent?.Invoke(item);
        }

        public void OnMoveRight(GuidableItem item)
        {
            onMoveRightEvent?.Invoke(item);
        }
    }
}

