using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    [Serializable]
    public class NavigationMap
    {
        public UIDefine.Group_ID group;
        public UIDefine.Panel_ID panel;
    }

    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(menuName= "MyMenu/Create NavigationConfig")]
    public class NavigationConfig : ScriptableObject
    {
        [SerializeField]
        private NavigationMap[] map;

        private Dictionary<UIDefine.Group_ID, UIDefine.Panel_ID> kv;

        public void Init() 
        {
            kv = new Dictionary<UIDefine.Group_ID, UIDefine.Panel_ID>();

            for (int i = 0; i < map.Length; i++)
            {
                NavigationMap nmp = map[i];
                kv[nmp.group] = nmp.panel;
            }
        }

        public UIDefine.Panel_ID GetMap(UIDefine.Group_ID group) 
        {
            if (kv.ContainsKey(group)) 
            {
                return kv[group];
            }
            else
            {
                return UIDefine.Panel_ID.Undefined;
            }
        }
    }
}
