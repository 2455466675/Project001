using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class ActionAssets : MonoBehaviour
	{
        public static ActionAssets commonActionAssets;

        public ActionAsset[] actionAssets;

        public static ActionAsset FindAction(string actionName)
        {
            if (commonActionAssets == null)
            {
                commonActionAssets = GameCore.System.Config.commonActionAssets;
            }
            if (commonActionAssets == null)
            {
                return null;
            }
            return Array.Find(commonActionAssets.actionAssets, a => a.actionName == actionName);
        }

        public ActionAsset GetAction(string actionName)
        {
            ActionAsset ac = null;

            if (actionAssets != null && actionAssets.Length > 0)
            {
                ac = Array.Find(actionAssets, a => a.actionName == actionName);
            }

            if (ac == null)
            {
                ac = FindAction(actionName);
            }

            return ac;
        }

        [Button("Init")]
        private void Init()
        {
            actionAssets = GetComponentsInChildren<ActionAsset>();
        }
    }
}

