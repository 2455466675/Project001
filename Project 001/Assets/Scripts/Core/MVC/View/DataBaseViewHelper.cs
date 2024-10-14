using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVC
{
    /// <summary>
    /// 
    /// </summary>
	public class DataBaseViewHelper : ViewHelper<DataBaseView>
    {
        protected override void OnUpdatedView()
        {
            Debug.Log($"DataBaseViewHelper-{view.MainField.GetStringValue()}");
        }
    }
}

