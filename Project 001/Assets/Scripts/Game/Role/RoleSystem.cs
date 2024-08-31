
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public void Init()
        {
            //roles = new Role[2];

            //roles[0] = new Role(1001);
            //roles[1] = new Role(1002);

            //roles[0].NextRole = roles[1];
            //roles[1].PreviousRole = roles[0];


            roles = new Role[5];

            roles[0] = new Role(1001);
            roles[1] = new Role(1002);
            roles[2] = new Role(1003);
            roles[3] = new Role(1004);
            roles[4] = new Role(1005);

            roles[0].NextRole = roles[1];
            roles[0].SetIsLeader(true);

            roles[1].PrevRole = roles[0];
            roles[1].NextRole = roles[2];
            roles[1].SetIsLeader(false);

            roles[2].PrevRole = roles[1];
            roles[2].NextRole = roles[3];
            roles[2].SetIsLeader(false);

            roles[3].PrevRole = roles[2];
            roles[3].NextRole = roles[4];
            roles[3].SetIsLeader(false);

            roles[4].PrevRole = roles[3];
            roles[4].SetIsLeader(false);
        }

        public void Run(bool isRunning)
        {
            for (int i = 0; i < roles.Length; i++)
            {
                roles[i].Run(isRunning);
            }
        }

        public void Move(float x, float y)
        {
            Leader.Move(x, y);            
        }

        public void Stop()
        {
            Leader.Move(0f, 0f);
        }
    }
}

