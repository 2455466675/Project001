using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationGroup : MonoBehaviour
    {
        [SerializeField]
        protected NavigationList[] list;

        public void Init()
        {
            list = GetComponentsInChildren<NavigationList>();
        }
    }
}
