using System;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Core;

namespace GameFramework.UI
{
    public enum GroupType 
    {
        Bottom = 1,
        Normal = 2,
        Top = 3,
        Log = 4,
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
        private Camera m_Camera;
        [SerializeField]
        private Canvas m_Canvas;
        [SerializeField]
        private CanvasGroup m_Mask;

        [SerializeField]
        private List<Group> m_Groups;

        private void Awake()
        {
            ShowMask();
        }

        public void ShowMask() 
        {
            if (m_Mask != null) 
            {
                m_Mask.alpha = 1f;
            }
        }

        public void HideMask() 
        {
            if (m_Mask != null)
            {
                m_Mask.alpha = 0f;
            }
        }

        public Transform GetGroup(GroupType type) 
        {
            if (m_Groups == null) 
            {
                return null;
            }

            var container = m_Groups.Find(x => x.type == type);
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
