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
        public class GuideItemUnityEvent : UnityEvent<GuidableItem>
        {
        }

        [SerializeField]
        private GuideItemUnityEvent onSubmitEvent;
        [SerializeField]
        private GuideItemUnityEvent onSelectEvent;
        [SerializeField]
        private GuideItemUnityEvent onDeselectEvent;
        [SerializeField]
        private GuideItemUnityEvent onOutFocusEvent;
        [SerializeField]
        private GuideItemUnityEvent onMoveUpEvent;
        [SerializeField]
        private GuideItemUnityEvent onMoveDownEvent;
        [SerializeField]
        private GuideItemUnityEvent onMoveLeftEvent;
        [SerializeField]
        private GuideItemUnityEvent onMoveRightEvent;

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

