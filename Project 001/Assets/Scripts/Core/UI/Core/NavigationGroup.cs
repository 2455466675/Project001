using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public enum GroupType
    {
        Vertical   = 0,
        Horizontal = 1,
        Grid       = 2,
    }

    /// <summary>
    /// 
    /// </summary>
    public class NavigationGroup : MonoBehaviour
    {
        [SerializeField]
        protected NavigationItem item;

        [SerializeField]
        protected int minIndex;
        [SerializeField]
        protected int maxIndex;
        [SerializeField]
        protected int pointer;

        [SerializeField]
        protected RectTransform viewport;
        [SerializeField]
        protected RectTransform content;
        [SerializeField]
        private CanvasGroup canvasGroup;

        /// <summary>
        /// 当显示时
        /// </summary>
        public virtual void OnShow() 
        {        
        }

        /// <summary>
        /// 当隐藏时
        /// </summary>
        public virtual void OnHide() 
        {
        }

        /// <summary>
        /// 当聚焦时
        /// </summary>
        public virtual void OnInFocus()
        {
        }

        /// <summary>
        /// 当失焦时
        /// </summary>
        public virtual void OnOutFocus()
        {
        }

        /// <summary>
        /// 移动指针指令
        /// </summary>
        /// <param name="dir"></param>
        public virtual void OnMove(Vector2 dir) 
        {        
        }

        /// <summary>
        /// 当确认按下时
        /// </summary>
        public virtual void OnSubmit() 
        {            
        }
    }
}
