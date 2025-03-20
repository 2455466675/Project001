using UnityEngine;

namespace Navigation
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationItem : MonoBehaviour
    {
        private object data;

        /// <summary>
        /// Ñ¡ÔñÆ÷
        /// </summary>
        [SerializeField]
        private NavigationItemSelector selector;
        /// <summary>
        /// ÊÂ¼þ
        /// </summary>
        [SerializeField]
        private NavigationItemEvent @event;

        public int Index { get; private set; }
        public virtual bool IsValid => true;

        public void SetData(object data)
        {
            this.data = data;
            OnRefresh();
        }

        public object GetData()
        {
            return data;
        }

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

        public void InitItem() 
        {
            if (@event == null) 
            {
                @event = GetComponentInChildren<NavigationItemEvent>();
            }

            if (selector == null) 
            {
                selector = GetComponentInChildren<NavigationItemSelector>();
            }
        }

        protected virtual void OnRefresh() { }
    }
}
