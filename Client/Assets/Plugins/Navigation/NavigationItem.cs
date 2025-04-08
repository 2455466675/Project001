using UnityEngine;

namespace Navigation
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationItem : MonoBehaviour
    {
        private object data;
        [SerializeField]
        private string param;

        public bool IsBinded => data is not null;
        public int Index { get; private set; }
        public virtual bool IsValid => true;

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

        public void BindData(object data) 
        {            
            this.data = data;
        }

        public void UnbindData() 
        {
            this.data = null;
        }

        public bool TryGetData<T>(out T result) where T : class
        {
            if (IsBinded && data is T r) 
            {
                result = r;
                return true;
            }
            else
            {
                result = default;
                return false;                
            }
        }

        public string GetStringParam() 
        {
            return param;
        }

        public int GetIntParam() 
        {
            if (int.TryParse(param, out var val)) 
            {
                return val;
            }
            return 0;
        }

        public double GetDoubleParam() 
        {
            if (double.TryParse(param, out var val)) 
            {
                return val;
            }
            return 0d;
        }

        internal void SetActive(bool active)
        {
            gameObject.SetActive(active);
            if (!active && IsBinded) 
            {
                UnbindData();
            }
        }

        internal void SetIndex(int index)
        {
            Index = index;
        }

        internal void OnSelect()
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

        internal void OnDeselect()
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

        internal void OutFocus()
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

        internal void OnSubmit()
        {
            if (@event != null)
            {
                @event.OnSubmit(this);
            }
        }

        internal void OnMoveUp() 
        {
            if (@event != null)
            {
                @event.OnMoveUp(this);
            }
        }

        internal void OnMoveDown()
        {
            if (@event != null)
            {
                @event.OnMoveDown(this);
            }
        }

        internal void OnMoveLeft()
        {
            if (@event != null)
            {
                @event.OnMoveLeft(this);
            }
        }

        internal void OnMoveRight()
        {
            if (@event != null)
            {
                @event.OnMoveRight(this);
            }
        }

        protected void InitItem() 
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
    }
}
