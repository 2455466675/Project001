using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class ActionGroup : MonoBehaviour
    {
        public string actionName;
        public ActorBaseAction[] actions;

        public void Awake()
        {
            CheckName();
        }

        public void Execute(Actor actor, params object[] actionArgs)
        {
            if (actions == null || actions.Length == 0) 
            {
                return;
            }

            foreach (var item in actions)
            {
                if(item == null) continue;
                item.Execute(actor, actionArgs);
            }
        }

        private void CheckName()
        {
            actionName = string.IsNullOrEmpty(actionName) ? name.Trim() : actionName.Trim();
        }

        [ContextMenu("Init")]
        private void Init()
        {
            actions = GetComponentsInChildren<ActorBaseAction>();
        }

        private void OnValidate()
        {
            CheckName();
        }
    }
}
