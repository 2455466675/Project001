using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.System
{
    public class ActionSource : MonoBehaviour
    {
        [SerializeField]
        private ActorAction[] actions;
        private bool isLoaded;

        public ActionPlayer CreatePlayer(int hash) 
        {
            ActorAction action = FindAction(hash);
            if (action == null)
            {
                return null;
            }

            ActionPlayer player = action.CreatePlayer();
            return player;
        }

        private ActorAction FindAction(int hash)
        {
            if (actions == null || actions.Length == 0)
            {
                return null;
            }

            for (int i = 0; i < actions.Length; i++)
            {
                ActorAction action = actions[i];

                if (action != null && action.ActionNameHash == hash) 
                {
                    return action;
                }
            }

            return null;
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