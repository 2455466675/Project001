using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation
{
    public interface INavigationElement
    {
        /// <summary>
        /// ¾Û½¹
        /// </summary>
        /// <returns></returns>
        bool InFocus(params int[] indexs);
        /// <summary>
        /// Ê§½¹
        /// </summary>
        /// <returns></returns>
        bool OutFocus();
        /// <summary>
        /// Ê§½¹ºóÖØÐÂ¾Û½¹
        /// </summary>
        /// <returns></returns>
        bool Refocus();
        /// <summary>
        /// ¹Ø±Õ
        /// </summary>
        void Close();
        /// <summary>
        /// ÒÆ¶¯
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        bool Move(Vector2 dir);
    }

    /// <summary>
    /// 
    /// </summary>
	//public class INavigationElement : MonoBehaviour
	//{

 //       public virtual void OnSelected()
 //       {

 //       }
 //       public virtual void OnDeselect()
 //       {

 //       }
 //       public virtual void OnSubmit()
 //       {

 //       }
 //       public virtual void MoveUp()
 //       {

 //       }
 //       public virtual void MoveDown()
 //       {

 //       }
 //       public virtual void MoveLeft()
 //       {

 //       }
 //       public virtual void MoveRight()
 //       {

 //       }
 //   }
}

