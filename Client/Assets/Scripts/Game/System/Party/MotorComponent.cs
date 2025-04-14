using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Game.System
{
    public enum MoveType 
    {
        IdleDown,
        IdleUp,
        IdleLeft,
        IdleRight,
        WalkDown,
        WalkUp,
        WalkLeft,
        WalkRight,
        RunDown,
        RunUp,
        RunLeft,
        RunRight,
    }

    public struct MoveTrace 
    {
        public MoveType moveType;
        public float x;
        public float y;
        public float deltaDistance;
    }

    /// <summary>
    /// 
    /// </summary>
    public class MotorComponent : UnitComponent, IAwakeComponent, IFixedUpdateComponent
    {
        private ActorComponent actorComponent;
        private QueueableComponent queueable;

        private float dirX;
        private float dirY;

        private float lastX;
        private float lastY;

        private bool isRunning;
        private bool isMoving;

        private float gap;
        private int frame;
        /// <summary>
        /// 距离前置角色的距离
        /// </summary>
        private float distance;
        /// <summary>
        /// 前置角色的足迹路径
        /// </summary>
        private List<MoveTrace> traces;

        public void Awake()
        {
            queueable = GetComponent<QueueableComponent>();
            actorComponent = GetComponent<ActorComponent>();
            dirX = 0f;
            dirY = -1f;
            gap = Game.Config.Formula.TEAM_GAP;
            frame = 0;
            isRunning = false;
            traces = new List<MoveTrace>();
        }

        public void Move(float x, float y)
        {
            MoveType moveType;
            string actionName;

            if (x != 0)
            {
                if (x > 0)
                {
                    if (isRunning)
                    {
                        actionName = "RunRight";
                        moveType = MoveType.RunRight;
                    }
                    else
                    {
                        actionName = "WalkRight";
                        moveType = MoveType.WalkRight;
                    }
                }
                else
                {
                    if (isRunning)
                    {
                        actionName = "RunLeft";
                        moveType = MoveType.RunLeft;
                    }
                    else
                    {
                        actionName = "WalkLeft";
                        moveType = MoveType.WalkLeft;
                    }
                }
                isMoving = true;
            }
            else if (y != 0)
            {
                if (y > 0)
                {
                    if (isRunning)
                    {
                        actionName = "RunUp";
                        moveType = MoveType.RunUp;
                    }
                    else
                    {
                        actionName = "WalkUp";
                        moveType = MoveType.WalkUp;
                    }
                }
                else
                {
                    if (isRunning)
                    {
                        actionName = "RunDown";
                        moveType = MoveType.RunDown;
                    }
                    else
                    {
                        actionName = "WalkDown";
                        moveType = MoveType.WalkDown;
                    }
                }
                isMoving = true;
            }
            else
            {
                if (dirX != 0f)
                {
                    if (dirX < 0f)
                    {
                        actionName = "IdleLeft";
                        moveType = MoveType.IdleLeft;
                    }
                    else
                    {
                        actionName = "IdleRight";
                        moveType = MoveType.IdleRight;
                    }
                }
                else
                {
                    if (dirY < 0f)
                    {
                        actionName = "IdleDown";
                        moveType = MoveType.IdleDown;
                    }
                    else
                    {
                        actionName = "IdleUp";
                        moveType = MoveType.IdleUp;
                    }
                }

                isMoving = false;
            }

            dirX = x;
            dirY = y;

            actorComponent.PlayAction(actionName);

            Vector2 pos = actorComponent.GetPosition();

            if (lastX != 0 || lastY != 0)
            {
                if (isMoving)
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
                            isMoving = false;
                            frame = 0;
                        }
                    }
                }
            }

            lastX = pos.x;
            lastY = pos.y;

            queueable.TransmitTrace(new MoveTrace() { moveType = moveType, x = pos.x, y = pos.y });
        }

        public void Run(bool isRunning)
        {
            this.isRunning = isRunning;
        }

        public bool PushTrace(MoveTrace trace)
        {
            if (trace.moveType == MoveType.IdleDown || trace.moveType == MoveType.IdleUp || trace.moveType == MoveType.IdleLeft || trace.moveType == MoveType.IdleRight)
            {
                return false;
            }

            //计算与上一个点的距离
            float d = 0f;
            if (traces.Count > 0)
            {
                MoveTrace last = traces[^1];
                MoveType moveType = trace.moveType;

                if (moveType == MoveType.WalkUp || moveType == MoveType.RunUp)
                {
                    d = trace.y - last.y;
                }
                else if (moveType == MoveType.WalkDown || moveType == MoveType.RunDown)
                {
                    d = trace.y - last.y;
                }
                else if (moveType == MoveType.WalkLeft || moveType == MoveType.RunLeft)
                {
                    d = trace.x - last.x;
                }
                else if (moveType == MoveType.WalkRight || moveType == MoveType.RunRight)
                {
                    d = trace.x - last.x;
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
            isMoving = true;

            return true;
        }

        /// <summary>
        /// 跟随移动
        /// </summary>
        private void Follow()
        {
            if (!isMoving)
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
                isMoving = true;
            }
            else
            {
                if (queueable.Prev.GetComponent<MotorComponent>().isMoving)   //如果前置角色已经开始移动了就不再停止
                {
                    return;
                }

                if (!isMoving)
                {
                    return;
                }

                string actionName;

                if (dirX != 0f)
                {
                    if (dirX < 0f)
                    {
                        actionName = "IdleLeft";
                    }
                    else
                    {
                        actionName = "IdleRight";
                    }
                }
                else
                {
                    if (dirY < 0f)
                    {
                        actionName = "IdleDown";
                    }
                    else
                    {
                        actionName = "IdleUp";
                    }
                }

                actorComponent.PlayAction(actionName);
                isMoving = false;
                MLog.Log($"actionName:{actionName}");
            }
        }

        /// <summary>
        /// 去下一个足迹点
        /// </summary>
        private void MoveToNextTrace()
        {
            if (traces.Count == 0)
            {
                return;
            }

            MoveTrace trace = traces[0];
            MoveType moveType = trace.moveType;

            string acName = string.Empty;
            if (moveType == MoveType.WalkUp || moveType == MoveType.RunUp)
            {
                acName = isRunning ? "FollowRunUp" : "FollowWalkUp";
                dirX = 0;
                dirY = 1;
            }
            else if (moveType == MoveType.WalkDown || moveType == MoveType.RunDown)
            {
                acName = isRunning ? "FollowRunDown" : "FollowWalkDown";
                dirX = 0;
                dirY = -1;
            }
            else if (moveType == MoveType.WalkLeft || moveType == MoveType.RunLeft)
            {
                acName = isRunning ? "FollowRunLeft" : "FollowWalkLeft";
                dirX = -1;
                dirY = 0;
            }
            else if (moveType == MoveType.WalkRight || moveType == MoveType.RunRight)
            {
                acName = isRunning ? "FollowRunRight" : "FollowWalkRight";
                dirX = 1;
                dirY = 0;
            }

            MLog.Log($"MoveToNextTrace : {acName}, {trace.x}, {trace.y}");
            distance -= trace.deltaDistance;
            traces.RemoveAt(0);

            actorComponent.PlayAction(acName);
            actorComponent.SetPosition(new Vector2(trace.x, trace.y));
            queueable.TransmitTrace(trace);
        }

        public void FixedUpdate(float dt)
        {
            Follow();
        }
    }
}
