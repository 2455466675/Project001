using System;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Core;

namespace GameFramework.View.UI
{
    public enum GroupType 
    {
        Bottom = 1,
        Normal = 2,
        Top = 3,
        Log = 4,

        Battle = 99,
    }

    [Serializable]
    public class Group 
    {
        public GroupType type;
        public Transform transform;
    }

    /// <summary>
    /// 
    /// </summary>
    public class UINode : GameNode
    {
        [SerializeField]
        private Camera UICamera;
        [SerializeField]
        private Canvas canvas;
        [SerializeField]
        private CanvasGroup mask;

        [SerializeField]
        private List<Group> groups;

        private void Awake()
        {
            ShowMask();
        }

        public void ShowMask() 
        {
            if (mask != null) 
            {
                mask.alpha = 1f;
            }
        }

        public void HideMask() 
        {
            if (mask != null)
            {
                mask.alpha = 0f;
            }
        }

        public Transform GetGroup(GroupType type) 
        {
            if (groups == null) 
            {
                return null;
            }

            var container = groups.Find(x => x.type == type);
            if (container == null) 
            {
                return null;
            }

            return container.transform;
        }

        public Transform GetGroup(int type)
        {
            GroupType gType;
            if (Enum.IsDefined(typeof(GroupType), type)) 
            {
                gType = (GroupType)type;
            }
            else
            {
                gType = GroupType.Normal;
            }

            return GetGroup(gType);
        }
    }
}
