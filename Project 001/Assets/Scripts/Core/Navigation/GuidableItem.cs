using UnityEngine;
using MVC;

namespace Navigation
{
    /// <summary>
    /// 
    /// </summary>
	public class GuidableItem : ListItem
    {
        /// <summary>
        /// Ñ¡ÔñÆ÷
        /// </summary>
        [SerializeField]
        private GuidableItemSelector selector;
        /// <summary>
        /// ÊÂ¼þ
        /// </summary>
        [SerializeField]
        private GuidableItemEvent @event;
        public bool IsBeSelected { get; }
        public virtual bool IsValid => true;
        public int Index { get; private set; }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public void SetIndex(int index)
        {
            Index = index;
        }

        public void OnSelect()
        {
            if (selector != null)
            {
                selector.OnSelect();
            }
            if (@event != null)
            {
                @event.OnSelect(this);
            }
        }

        public void OnDeselect()
        {
            if (selector != null)
            {
                selector.OnDeselect();
            }
            if (@event != null)
            {
                @event.OnDeselect(this);
            }
        }

        public void OutFocus()
        {
            if (selector != null)
            {
                selector.OnOutFocus();
            }
            if (@event != null)
            {
                @event.OnOutFocus(this);
            }
        }

        public void OnSubmit()
        {
            if (@event != null)
            {
                @event.OnSubmit(this);
            }
        }
    }
}

