using Game.Cfg;
using Game.Core;
using OfficeOpenXml.ConditionalFormatting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.UI
{
    [Serializable]
    public class WinGroupItem 
    {
        public UIGroup group;
        public Transform winGroup;
    }

    /// <summary>
    /// 
    /// </summary>
    public class UIRoot : MonoBehaviour
    {
        public Camera UICamera;
        public WinGroupItem[] groups;
      
        private Dictionary<UIGroup, Transform> groupsMap;
    
        public void Awake()
        {
            DontDestroyOnLoad(this);
            UICamera = GetComponentInChildren<Camera>();
            InitWinGroup();
        }

        public Transform GetWinGroup(UIGroup group)
        {
            if (!groupsMap.ContainsKey(group))
            {
                return null;
            }
            return groupsMap[group];
        }

        private void InitWinGroup()
        {
            if (groups != null && groups.Length > 0)
            {
                groupsMap = new Dictionary<UIGroup, Transform>();
                for (int i = 0; i < groups.Length; i++)
                {
                    groupsMap.Add(groups[i].group, groups[i].winGroup);
                }
            }
        }
    }
}