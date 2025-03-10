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
            CheckName();
        }

        public void Execute(Actor actor)
        {
            foreach (var item in actions)
            {
                item.Execute(actor);
            }
        }

        private void CheckName() 
        {
            actionName = string.IsNullOrEmpty(actionName) ? name.Trim() : actionName.Trim();
        }

        [ContextMenu("Init")]
        private void Init()
        {
            actions = GetComponentsInChildren<BaseAction>();
        }

        private void OnValidate()
        {
            CheckName();
        }
    }
}

