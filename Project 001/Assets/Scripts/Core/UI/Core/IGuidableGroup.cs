using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
	public interface IGuidableGroup
	{
        int Layer { get; set; }
        bool IsFocus { get; }
        /// <summary>
        /// ¾Û½¹
        /// </summary>
        void InFocus();
        /// <summary>
        /// Ê§½¹
        /// </summary>
        void OutFocus();
        /// <summary>
        /// ÍË³ö
        /// </summary>
        void Exit();
        void OnMoveToUp();
        void OnMoveToDown();
        void OnMoveToLeft();
        void OnMoveToRight();
    }
}

