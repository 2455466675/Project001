using UnityEngine;

namespace Navigation
{
    /// <summary>
    /// 导航元素选中表现基类：负责选中/取消选中/失焦时的视觉表现，供子类重写实现高亮、缩放等效果。
    /// </summary>
    public class NavigationItemSelector : MonoBehaviour
    {
        /// <summary>
        /// 选中
        /// </summary>
        public virtual void OnSelect()
        {

        }
        /// <summary>
        /// 取消选中
        /// </summary>
        public virtual void OnDeselect()
        {

        }
        /// <summary>
        /// 失焦
        /// </summary>
        public virtual void OnOutFocus()
        {

        }
    }
}
