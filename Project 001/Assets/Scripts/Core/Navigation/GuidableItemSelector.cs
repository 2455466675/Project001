using UnityEngine;

namespace Navigation
{
    /// <summary>
    /// 
    /// </summary>
	public class GuidableItemSelector : MonoBehaviour
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

        private void OnDisable()
        {
            OnDeselect();
        }
    }
}

