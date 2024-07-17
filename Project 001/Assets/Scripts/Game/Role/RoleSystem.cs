
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class RoleSystem : IGameSystem
	{

        public Role[] roles;

        public Role Leader => roles != null && roles.Length > 0 ? roles[0] : null;

        public Queue<string> actionNames = new Queue<string>();

        public IEnumerator LateUpdate()
        {
            yield return new WaitForSeconds(1f);
            while (actionNames.Count > 0)
            {
                string name = actionNames.Dequeue();
                roles[1].Actor.PlayAction(name);
                yield return null;
            }
        }


        public void Init()
        {
            roles = new Role[2];

            MLog.Log("RoleSystem Init");

            roles[0] = new Role(1001);
            roles[1] = new Role(1002);
        }

        public void Move(float x, float y)
        {
            Leader.Move(x, y);
        }
    }
}

