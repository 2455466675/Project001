using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class ActionAsset : MonoBehaviour
	{
        public string actionName;
        public BaseAction[] actions;

        public void Awake()
        {
            actionName = string.IsNullOrEmpty(actionName) ? name.Trim() : actionName;
        }

        public void Execute(Actor actor)
        {
            foreach (var item in actions)
            {
                item.Execute(actor);
            }
        }

        [ContextMenu("Init")]
        private void Init()
        {
            actions = GetComponentsInChildren<BaseAction>();
        }
    }
}

