using EC;
using Game.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class MotorComponent : EC.Component, IAwake, ILateUpdate
    {
        /// <summary>
        /// 是否正在移动
        /// </summary>
        public bool IsMoving { get; private set; }
        /// <summary>
        /// 是否正在奔跑
        /// </summary>
        public bool IsRunning { get; set; }

        private ActorComponent actor;
        private QueueableComponent queueable;

        /// <summary>
        /// 距离前置角色的距离
        /// </summary>
        private float distance;
        /// <summary>
        /// 前置角色的足迹路径
        /// </summary>
        private List<MoveTrace> traces;

        private float gap;

        private int frame;
        private float lastX;
        private float lastY;

        public void Awake()
        {
            traces = new List<MoveTrace>();
            actor = MyEntity.GetComponent<ActorComponent>();
            queueable = MyEntity.GetComponent<QueueableComponent>();
            gap = MyWorld.GetComponent<ConfigComponent>().Formula.TEAM_GAP;
        }

        public void LateUpdate(float dt)
        {
            Follow();
        }

        public void Move(Vector2 dir)
        {
            MoveType moveType;
            string actionName;

            float x = dir.x;
            float y = dir.y;

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

            Vector2 pos = actor.GetPosition();

            if (lastX != 0 || lastY != 0) 
            {
                if (IsMoving) 
                {
                    float _x = pos.x - lastX;
                    float _y = pos.y - lastY;
                    if (_x * _x + _y * _y < 0.01f)
                    {
                        if (frame < 5)
                        {
                            frame++;
                        }
                        else
                        {
                            IsMoving = false;
                            frame = 0;
                        }
                    }
                }
            }
            
            lastX = pos.x;
            lastY = pos.y;

            queueable.TransmitTrace(new MoveTrace() { moveType = moveType, position = pos });
        }

        /// <summary>
        /// 添加一个足迹点
        /// </summary>
        /// <param name="trace"></param>
        /// <returns>是否产生了移动</returns>
        public bool PushTrace(MoveTrace trace)
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

            if (queueable.IsLeader)
            {
                return;
            }

            if (distance >= gap) //距离前置角色一定距离时，开始跟随
            {
                MoveToNextTrace();
                IsMoving = true;
            }
            else
            {
                if (queueable.Prev.GetComponent<MotorComponent>().IsMoving)   //如果前置角色已经开始移动了就不再停止
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
        private void MoveToNextTrace()
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
                acName = IsRunning ? "FollowRunUp" : "FollowWalkUp";
            }
            else if (moveType == MoveType.Down || moveType == MoveType.RunDown)
            {
                acName = IsRunning ? "FollowRunDown" : "FollowWalkDown";
            }
            else if (moveType == MoveType.Left || moveType == MoveType.RunLeft)
            {
                acName = IsRunning ? "FollowRunLeft" : "FollowWalkLeft";
            }
            else if (moveType == MoveType.Right || moveType == MoveType.RunRight)
            {
                acName = IsRunning ? "FollowRunRight" : "FollowWalkRight";
            }
            else
            {
                acName = "Idle";
            }

            distance -= trace.deltaDistance;
            traces.RemoveAt(0);

            actor.PlayAction(acName);
            actor.SetPosition(trace.position);
            queueable.TransmitTrace(trace);
        }
    }
}
