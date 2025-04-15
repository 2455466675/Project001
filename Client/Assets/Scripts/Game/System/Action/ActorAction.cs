using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class ActorAction : MonoBehaviour
    {
        public int ActionNameHash => Common.StringToHash(actionName);

        [SerializeField]
        private string actionName;

        [SerializeField]
        private ActionItemBase[] items;

        public ActionPlayer CreatePlayer()
        {
            List<ActionCommandBase> commands = new List<ActionCommandBase>();

            if (items != null) 
            {
                ActionItemBase[] temp = items.Where(a => a != null).ToArray();

                int count = temp.Length;
                for (int i = 0; i < count; i++)
                {
                    ActionItemBase item = items[i];
                    commands.Add(item.CreateCommand());
                }
            }
                  
            ActionPlayer player = new ActionPlayer(ActionNameHash, commands.ToArray());
            return player;
        }

        [Button("Init")]
        public void Init() 
        {
#if UNITY_EDITOR
            items = GetComponentsInChildren<ActionItemBase>();

            if (string.IsNullOrEmpty(actionName)) 
            {
                actionName = gameObject.name;
            }
#endif
        }
    }
}
