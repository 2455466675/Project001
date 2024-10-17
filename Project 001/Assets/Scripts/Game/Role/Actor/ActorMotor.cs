using System.Collections.Generic;
using UnityEngine;

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
        /// <summary>
        /// 移动方向
        /// </summary>
        public MoveType moveType;
        /// <summary>
        /// 当前位置
        /// </summary>
        public Vector2 position;
        /// <summary>
        /// 相对上一个点的距离
        /// </summary>
        public float deltaDistance;
    }

    /// <summary>
    /// 角色移动类，负责角色的移动
    /// </summary>
	public class ActorMotor : MonoBehaviour
	{
        /// <summary>
        /// 是否正在移动
        /// </summary>
        public bool IsMoving { get; set; }
        /// <summary>
        /// 是否正在奔跑
        /// </summary>
        public bool IsRunning { get; set; }

        private Actor actor;

        /// <summary>
        /// 距离前置角色的距离
        /// </summary>
        private float distance;
        /// <summary>
        /// 前置角色的足迹路径
        /// </summary>
        private List<MoveTrace> traces;

        public void Start()
        {
            actor = GetComponent<Actor>();
            traces = new List<MoveTrace>();
        }

        public void FixedUpdate()
        {
            Follow();
        }

        public void Move(float x, float y)
        {
            MoveType moveType;
            string actionName;

            if (x != 0)
            {
                if (x > 0)
                {
                    if (IsRunning)
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
                    if (IsRunning)
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
                IsMoving = true;
            }
            else if (y != 0)
            {
                if (y > 0)
                {
                    if (IsRunning)
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
                    if (IsRunning)
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
                IsMoving = true;
            }
            else
            {
                actionName = "Idle";
                moveType = MoveType.Idle;
                IsMoving = false;
            }

            actor.PlayAction(actionName);
            IsMoving = actor.AddNextRoleTrace(new MoveTrace() {moveType = moveType, position = actor.transform.position});
        }
   
        /// <summary>
        /// 添加一个足迹点
        /// </summary>
        /// <param name="trace"></param>
        /// <returns>是否产生了移动</returns>
        public bool AddTrace(MoveTrace trace)
        {
            if (trace.moveType == MoveType.Idle)
            {
                return false;
            }

            //计算与上一个点的距离
            float d = 0f;
            if (traces.Count > 0)
            {
                MoveTrace last = traces[^1];
                MoveType moveType = trace.moveType;

                if (moveType == MoveType.Up || moveType == MoveType.RunUp)
                {
                    d = trace.position.y - last.position.y;
                }
                else if (moveType == MoveType.Down || moveType == MoveType.RunDown)
                {
                    d = trace.position.y - last.position.y;
                }
                else if (moveType == MoveType.Left || moveType == MoveType.RunLeft)
                {
                    d = trace.position.x - last.position.x;
                }
                else if (moveType == MoveType.Right || moveType == MoveType.RunRight)
                {
                    d = trace.position.x - last.position.x;
                }

                d = Mathf.Abs(d);

                if (d < 0.01f)
                {
                    return false;
                }
            }
            trace.deltaDistance = d;

            traces.Add(trace);
            distance += d;
            IsMoving = true;

            return true;
        }

        /// <summary>
        /// 跟随移动
        /// </summary>
        private void Follow()
        {
            if (!IsMoving)
            {
                return;
            }

            if (actor.IsLeader)
            {
                return;
            }

            if (distance >= GameCore.Cfg.Formula.TEAM_GAP) //距离前置角色一定距离时，开始跟随
            {
                MoveNextTrace();
                IsMoving = true;
            }
            else
            {
                if (actor.PrevActor.IsMoving)   //如果前置角色已经开始移动了就不再停止
                {
                    return;
                }

                if (!IsMoving)
                {
                    return;
                }
                actor.PlayAction("Idle");
                IsMoving = false;
            }
        }

        /// <summary>
        /// 去下一个足迹点
        /// </summary>
        private void MoveNextTrace()
        {
            if (traces.Count <= 0)
            {
                return;
            }

            MoveTrace trace = traces[0];

            string acName;
            MoveType moveType = trace.moveType;
            if (moveType == MoveType.Up || moveType == MoveType.RunUp)
            {
                acName = IsRunning ? "RunUp_2" : "WalkUp_2";
            }
            else if (moveType == MoveType.Down || moveType == MoveType.RunDown)
            {
                acName = IsRunning ? "RunDown_2" : "WalkDown_2";
            }
            else if (moveType == MoveType.Left || moveType == MoveType.RunLeft)
            {
                acName = IsRunning ? "RunLeft_2" : "WalkLeft_2";
            }
            else if (moveType == MoveType.Right || moveType == MoveType.RunRight)
            {
                acName = IsRunning ? "RunRight_2" : "WalkRight_2";
            }
            else
            {
                acName = "Idle";
            }

            actor.SetPosition(trace.position);
            actor.PlayAction(acName);
            distance -= trace.deltaDistance;
            traces.RemoveAt(0);

            actor.AddNextRoleTrace(trace);
        }
    }
}

