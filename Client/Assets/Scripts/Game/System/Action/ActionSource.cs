using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Game.System
{
    public class ActionSource : MonoBehaviour
    {
        [SerializeField]
        private ActorAction[] actions;

        public ActorAction FindAction(string actionName) 
        {
            if (actions == null || actions.Length == 0)
            {
                return null;
            }

            return Array.Find(actions, a => a != null && a.name == actionName);
        }

        [Button("Init")]
        private void Init() 
        {
            actions = GetComponentsInChildren<ActorAction>();
            foreach (var item in actions)
            {
                item.Init();
            }
        }
    }
}