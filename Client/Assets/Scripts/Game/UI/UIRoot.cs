using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public enum GroupType 
    {
        Bottom = 1,
        Normal = 2,
        Top = 3,
        Log = 4,
    }

    [Serializable]
    public class GroupContainer 
    {
        public GroupType type;
        public Transform transform;
    }

    /// <summary>
    /// 
    /// </summary>
    public class UIRoot : MonoBehaviour
    {
        public Camera UICamera;
        public List<GroupContainer> containers;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public Transform GetGroupContainer(GroupType type) 
        {
            if (containers == null) 
            {
                return null;
            }

            var container = containers.Find(x => x.type == type);
            if (container == null) 
            {
                return null;
            }

            return container.transform;
        }

        public Transform GetGroupContainer(int type)
        {
            if (containers == null)
            {
                return null;
            }
            
            GroupType gType;
            if (type <= 0) 
            {
                gType = GroupType.Normal;
            }
            else
            {
                gType = (GroupType)type;
            }

            var container = containers.Find(x => x.type == gType);
            if (container == null)
            {
                return null;
            }

            return container.transform;
        }
    }
}
