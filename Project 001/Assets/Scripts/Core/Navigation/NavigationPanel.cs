using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;

namespace Navigation
{
    /// <summary>
    /// 
    /// </summary>
	public class NavigationPanel : MonoBehaviour
    {
        [SerializeField]
        public List<NavigationList> lists;

        public NavigationList Find(ListName listName)
        {
            return lists.Find((l) => l.listName == listName);
        }
    }
}

