using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class ActionContainer : MonoBehaviour
    {
        public ActionGroup[] actionGroups;

        public ActionGroup GetAction(string actionName)
        {
            ActionGroup ac = null;
            if (actionGroups != null && actionGroups.Length > 0)
            {
                ac = Array.Find(actionGroups, a => a.actionName == actionName);
            }
            return ac;
        }

        [Button("Init")]
        private void Init()
        {
            actionGroups = GetComponentsInChildren<ActionGroup>();
        }
    }
}
