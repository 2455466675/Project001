using UnityEngine;

namespace Navigation
{
    public interface INavigationElement
    {
        /// <summary>
        /// 聚焦
        /// </summary>
        /// <returns>是否成功</returns>
        bool InFocus(params int[] indexs);
        /// <summary>
        /// 失焦
        /// </summary>
        /// <returns>是否成功</returns>
        bool OutFocus();
        /// <summary>
        /// 失焦后重新聚焦
        /// </summary>
        /// <returns>是否成功</returns>
        bool Refocus();
        /// <summary>
        /// 退出 
        /// </summary>
        void Exit();
        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="dir"></param>
        /// <returns>是否成功</returns>
        bool Move(Vector2 dir);
    }
}

