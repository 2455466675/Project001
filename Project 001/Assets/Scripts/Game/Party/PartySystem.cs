using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// ∂”ŒÈœµÕ≥
    /// </summary>
	public class PartySystem : DataProxy, IGameSystem
    {
        public PartyRole[] roles;
        public PartyRole Leader => roles != null && roles.Length > 0 ? roles[0] : null;

        public PartySystem(DataContainer container) : base(container)
        {
        }

        public void Init()
        {
            int count = 5;
            roles = new PartyRole[count];

            DataCollection datas = CreateCollection("Roles");

            for (int i = 0; i < count; i++)
            {
                DataContainer item = datas.Append();
                roles[i] = new PartyRole(1001 + i, item);
            }

            roles[0].IsLeader = true;
            roles[0].NextRole = roles[1];

            roles[1].PrevRole = roles[0];
            roles[1].NextRole = roles[2];

            roles[2].PrevRole = roles[1];
            roles[2].NextRole = roles[3];

            roles[3].PrevRole = roles[2];
            roles[3].NextRole = roles[4];

            roles[4].PrevRole = roles[3];
        }

        public void Run(bool isRunning)
        {
            for (int i = 0; i < roles.Length; i++)
            {
                roles[i].Run(isRunning);
            }
        }

        public void Move(Vector2 dir)
        {
            Leader.Move(dir);
        }

        public void Stop()
        {
            Leader.Move(new Vector2(0, 0));
        }
    }
}

