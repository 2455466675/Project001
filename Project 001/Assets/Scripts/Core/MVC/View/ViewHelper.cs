using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVC
{
    /// <summary>
    /// 
    /// </summary>
	public class ViewHelper<T> : MonoBehaviour where T : View
	{
        public T view;

        protected virtual void Start()
        {
            if (view == null)
            {
                view = GetComponent<T>();
            }
            if (view == null)
            {
                return;
            }
            view.Bind(OnUpdatedView);
        }

        protected virtual void OnUpdatedView()
        {

        }
    }
}

