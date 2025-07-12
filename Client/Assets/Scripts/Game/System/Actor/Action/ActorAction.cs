using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.GSystem
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
        private ActionCommand[] commands;

        public ActionPlayer CreatePlayer()
        {
            List<ActionCommandExecutor> executors = new List<ActionCommandExecutor>();

            if (this.commands != null) 
            {
                ActionCommand[] temp = this.commands.Where(a => a != null).ToArray();

                int count = temp.Length;
                for (int i = 0; i < count; i++)
                {
                    ActionCommand cmd = this.commands[i];
                    executors.Add(cmd.CreateExecutor());
                }
            }
                  
            ActionPlayer player = new ActionPlayer(ActionNameHash, executors.ToArray());
            return player;
        }

        [Button("Init")]
        public void Init() 
        {
#if UNITY_EDITOR
            commands = GetComponentsInChildren<ActionCommand>();

            if (string.IsNullOrEmpty(actionName)) 
            {
                actionName = gameObject.name;
            }
#endif
        }
    }
}
