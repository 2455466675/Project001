
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.System
{

    public enum MoveDir
    {
        Up       = 1,
        RunUp    = 9,

        Down     = 2,
        RunDown  = 8,

        Left     = 3,
        RunLeft  = 7,

        Right    = 4,
        RunRight = 6,

        Idle     = 5,
    }

    public struct MovePoint
    {
        public string acName;
        public MoveDir dir;
        public Vector2 pos;
        public bool isTurn;
        public float deltaDis;
    }

    /// <summary>
    /// 
    /// </summary>
	public class RoleSystem : IGameSystem
    {
        public Role[] roles;

        public Role Leader => roles != null && roles.Length > 0 ? roles[0] : null;

        public bool IsMove => Leader != null && Leader.IsMove;

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

            roles[1].PreviousRole = roles[0];
            roles[1].NextRole = roles[2];

            roles[2].PreviousRole = roles[1];
            roles[2].NextRole = roles[3];

            roles[3].PreviousRole = roles[2];
            roles[3].NextRole = roles[4];

            roles[4].PreviousRole = roles[3];
        }

        public void AddSpeed(bool isRun)
        {
            for (int i = 0; i < roles.Length; i++)
            {
                roles[i].IsRun = isRun;
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

