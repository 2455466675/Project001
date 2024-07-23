
using Game.Cfg;
using Game.Core;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.System
{
    public enum MoveType
    {
        Up = 1,
        RunUp = 9,

        Down = 2,
        RunDown = 8,

        Left = 3,
        RunLeft = 7,

        Right = 4,
        RunRight = 6,

        Idle = 5,
    }

    public struct MoveTrace
    {
        public MoveType moveType;
        public Vector2 position;
        public float deltaDistance;
    }

    /// <summary>
    /// 
    /// </summary>
	public class Role
	{
        public Actor Actor { get; private set; }

        public RoleData RoleData { get; private set; }

        public RoleCfg Cfg { get; private set; }

        public IntDB ID { get; private set; }

        public StringDB Name { get; private set; }

        public bool IsLeader;

        public bool IsMove;

        public bool IsRun;

        public Role PreviousRole;

        public Role NextRole;

        public List<MoveTrace> traces;

        private float distance;

        public Role(int id)
        {
            RoleCfg cfg = GameCore.GameCfg.Find<RoleCfg>(id);
            if (cfg == null)
            {
                return;
            }

            GameObject obj = GameCore.ResourceManager.LoadAsset<GameObject>(cfg.PrefabPath);
            GameObject actorObj = GoHelper.Instantiate(obj, GameCore.Scene.GetContainer(cfg.Container).container);
            Actor = actorObj.GetComponent<Actor>();

            Actor.role = this;

            ID = new IntDB(cfg.Id);
            Name = new StringDB(cfg.Name);
            Cfg = cfg;

            traces = new List<MoveTrace>();
        }

        public void AddTrace(MoveTrace trace)
        {
            if (trace.moveType == MoveType.Idle)
            {
                return;
            }

            float deltaDistance = 0f;
            if (traces.Count > 0)
            {
                MoveTrace last = traces[^1];
                MoveType moveType = trace.moveType;

                if (moveType == MoveType.Up || moveType == MoveType.RunUp)
                {
                    deltaDistance = trace.position.y - last.position.y;
                }
                else if (moveType == MoveType.Down || moveType == MoveType.RunDown)
                {
                    deltaDistance = trace.position.y - last.position.y;
                }
                else if (moveType == MoveType.Left || moveType == MoveType.RunLeft)
                {
                    deltaDistance = trace.position.x - last.position.x;
                }
                else if (moveType == MoveType.Right || moveType == MoveType.RunRight)
                {
                    deltaDistance = trace.position.x - last.position.x;
                }
            }            
            deltaDistance = Mathf.Abs(deltaDistance);

            trace.deltaDistance = deltaDistance;

            this.distance += deltaDistance;

            traces.Add(trace);
            IsMove = true;
        }

        public void Follow()
        {
            if (PreviousRole == null)
            {
                return;
            }

            if (!IsMove)
            {
                return;
            }

            if (distance >= 1f)
            {              
                MoveNextTrace();
                IsMove = true;
            }
            else
            {
                if (PreviousRole.IsMove)
                {
                    return;
                }
                if (!IsMove)
                {
                    return;
                }
                Actor.PlayAction("Idle");
                IsMove = false;
            }
        }

        public void MoveNextTrace()
        {
            MoveTrace trace = traces[0];

            string acName;
            MoveType moveType = trace.moveType;
            if (moveType == MoveType.Up || moveType == MoveType.RunUp)
            {
                acName = IsRun ? "RunUp_2" : "WalkUp_2";
            }
            else if (moveType == MoveType.Down || moveType == MoveType.RunDown)
            {
                acName = IsRun ? "RunDown_2" : "WalkDown_2";
            }
            else if (moveType == MoveType.Left || moveType == MoveType.RunLeft)
            {
                acName = IsRun ? "RunLeft_2" : "WalkLeft_2";
            }
            else if (moveType == MoveType.Right || moveType == MoveType.RunRight)
            {
                acName = IsRun ? "RunRight_2" : "WalkRight_2";
            }
            else
            {
                acName = "Idle";
            }

            Actor.PlayAction(acName);
            Actor.transform.position = trace.position;

            this.distance -= trace.deltaDistance;

            traces.RemoveAt(0);

            NextRole?.AddTrace(trace);
        }

        public void Move(float x, float y)
        {
            MoveType moveType;
            string actionName;

            if (x != 0)
            {
                if (x > 0)
                {
                    if (IsRun)
                    {
                        actionName = "RunRight";
                        moveType = MoveType.RunRight;
                    }
                    else
                    {
                        actionName = "WalkRight";
                        moveType = MoveType.Right;
                    }
                }
                else
                {
                    if (IsRun)
                    {
                        actionName = "RunLeft";
                        moveType = MoveType.RunLeft;
                    }
                    else
                    {
                        actionName = "WalkLeft";
                        moveType = MoveType.Left;
                    }
                }
                IsMove = true;
            }
            else if (y != 0)
            {
                if (y > 0)
                {
                    if (IsRun)
                    {
                        actionName = "RunUp";
                        moveType = MoveType.RunUp;
                    }
                    else
                    {
                        actionName = "WalkUp";
                        moveType = MoveType.Up;
                    }
                }
                else
                {
                    if (IsRun)
                    {   
                        actionName = "RunDown";
                        moveType = MoveType.RunDown;
                    }
                    else
                    {
                        actionName = "WalkDown";
                        moveType = MoveType.Down;
                    }
                }
                IsMove = true;
            }
            else
            {   
                actionName = "Idle";
                moveType = MoveType.Idle;
                IsMove = false;
            }
     
            Actor.PlayAction(actionName);

            NextRole?.AddTrace(new MoveTrace() { moveType = moveType, position = Actor.transform.position });   
        }     
    }
}