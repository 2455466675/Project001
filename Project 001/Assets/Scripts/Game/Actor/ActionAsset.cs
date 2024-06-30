using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
	public class ActionAsset : MonoBehaviour
	{
        public string actionName;
        public BaseAction[] actions;

        public void SetActor(Actor actor)
        {
            foreach (var item in actions)
            {
                item.SetActor(actor);
            }
        }

        public void Execute()
        {
            foreach (var item in actions)
            {
                item.Execute();
            }
        }

        [ContextMenu("Init")]
        private void Init()
        {
            actions = GetComponentsInChildren<BaseAction>();
        }
    }
}

