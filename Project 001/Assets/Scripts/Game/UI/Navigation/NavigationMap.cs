using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    [Serializable]
    public class ListGroupMap
    {
        public NavigationListDefine listDefine;
        public NavigationGroupDefine groupDefine;
    }

    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(menuName= "MyMenu/Create NavigationMap")]
    public class NavigationMap : ScriptableObject
    {
        [ReadOnly]
        [SerializeField]
        private List<ListGroupMap> list;

        public void AddMap(NavigationListDefine listDefine, NavigationGroupDefine groupDefine) 
        {
            if (list == null) 
            {
                list = new List<ListGroupMap>();
            }

            ListGroupMap map = list.Find(m => m.listDefine == listDefine);
            if (map != null) 
            {
                map.groupDefine = groupDefine;
            }
            else
            {
                map = new ListGroupMap();
                map.listDefine = listDefine;
                map.groupDefine = groupDefine;
                list.Add(map);
            }
        }

        public NavigationGroupDefine GetGroupDefine(NavigationListDefine listDefine) 
        {
            ListGroupMap map = list.Find(m => m.listDefine == listDefine);
            if (map != null)
            {
                return map.groupDefine;
            }
            else
            {
                return NavigationGroupDefine.Undefined;
            }
        }

        [Button("Clear")]
        private void Clear() 
        {
            list?.Clear();
        }
    }
}
