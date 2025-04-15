using System.Collections.Generic;
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
        private static readonly int walkUpHash = Common.StringToHash("WalkUp");
        private static readonly int walkDownHash = Common.StringToHash("WalkDown");
        private static readonly int walkLeftHash = Common.StringToHash("WalkLeft");
        private static readonly int walkRightHash = Common.StringToHash("WalkRight");
        private static readonly int runUpHash = Common.StringToHash("RunUp");
        private static readonly int runDownHash = Common.StringToHash("RunDown");
        private static readonly int runLeftHash = Common.StringToHash("RunLeft");
        private static readonly int runRightHash = Common.StringToHash("RunRight");
        private static readonly int idleUpHash = Common.StringToHash("IdleUp");
        private static readonly int idleDownHash = Common.StringToHash("IdleDown");
        private static readonly int idleLeftHash = Common.StringToHash("IdleLeft");
        private static readonly int idleRightHash = Common.StringToHash("IdleRight");
        private static readonly int followWalkUpHash = Common.StringToHash("FollowWalkUp");
        private static readonly int followWalkDownHash = Common.StringToHash("FollowWalkDown");
        private static readonly int followWalkLeftHash = Common.StringToHash("FollowWalkLeft");
        private static readonly int followWalkRightHash = Common.StringToHash("FollowWalkRight");
        private static readonly int followRunUpHash = Common.StringToHash("FollowRunUp");
        private static readonly int followRunDownHash = Common.StringToHash("FollowRunDown");
        private static readonly int followRunLeftHash = Common.StringToHash("FollowRunLeft");
        private static readonly int followRunRightHash = Common.StringToHash("FollowRunRight");

        private ActorComponent actorComponent;
        private PartyComponent partyComponent;

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
            partyComponent = GetComponent<PartyComponent>();
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
            MoveType moveType = DoAction(x, y);
            Transmit(moveType);
            Block();
        }

        private MoveType DoAction(float x, float y)
        {
            MoveType moveType;
            int actionHash;

            if (x == 0f && y == 0f) 
            {
                if (dirX != 0f)
                {
                    if (dirX < 0f)
                    {
                        actionHash = idleLeftHash;
                        moveType = MoveType.IdleLeft;
                    }
                    else
                    {
                        actionHash = idleRightHash;
                        moveType = MoveType.IdleRight;
                    }
                }
                else
                {
                    if (dirY < 0f)
                    {
                        actionHash = idleDownHash;
                        moveType = MoveType.IdleDown;
                    }
                    else
                    {
                        actionHash = idleUpHash;
                        moveType = MoveType.IdleUp;
                    }
                }

                isMoving = false;
            }
            else
            {
                if (x != 0)
                {
                    if (x > 0)
                    {
                        if (isRunning)
                        {
                            actionHash = runRightHash;
                            moveType = MoveType.RunRight;
                        }
                        else
                        {
                            actionHash = walkRightHash;
                            moveType = MoveType.WalkRight;
                        }
                    }
                    else
                    {
                        if (isRunning)
                        {
                            actionHash = runLeftHash;
                            moveType = MoveType.RunLeft;
                        }
                        else
                        {
                            actionHash = walkLeftHash;
                            moveType = MoveType.WalkLeft;
                        }
                    }
                    isMoving = true;
                }
                else
                {
                    if (y > 0)
                    {
                        if (isRunning)
                        {
                            actionHash = runUpHash;
                            moveType = MoveType.RunUp;
                        }
                        else
                        {
                            actionHash = walkUpHash;
                            moveType = MoveType.WalkUp;
                        }
                    }
                    else
                    {
                        if (isRunning)
                        {
                            actionHash = runDownHash;
                            moveType = MoveType.RunDown;
                        }
                        else
                        {
                            actionHash = walkDownHash;
                            moveType = MoveType.WalkDown;
                        }
                    }
                }
                dirX = x;
                dirY = y;
                isMoving = true;
            }
            
            actorComponent.PlayAction(actionHash);
            return moveType;
        }

        private void Transmit(MoveType moveType) 
        {
            Vector2 pos = actorComponent.GetPosition();
            partyComponent.TransmitTrace(new MoveTrace() { moveType = moveType, x = pos.x, y = pos.y });
        }

        private void Block() 
        {
            if (!isMoving)
            {
                return;
            }

            Vector2 pos = actorComponent.GetPosition();
            if (lastX != 0 || lastY != 0)
            {
                float _x = pos.x - lastX;
                float _y = pos.y - lastY;
                if (_x * _x + _y * _y < 0.01f)
                {
                    if (frame < 3)
                    {
                        frame++;
                    }
                    else
                    {
                        isMoving = false;
                        frame = 0;
                    }
                }
                else
                {
                    frame = 0;
                }
            }

            lastX = pos.x;
            lastY = pos.y;
        }

        public void Run(bool isRunning)
        {
            this.isRunning = isRunning;
        }

        public void PushTrace(MoveTrace trace)
        {
            if (trace.moveType == MoveType.IdleDown || trace.moveType == MoveType.IdleUp || trace.moveType == MoveType.IdleLeft || trace.moveType == MoveType.IdleRight)
            {
                return;
            }

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
                    return;
                }
            }
            trace.deltaDistance = d;

            traces.Add(trace);
            distance += d;
            isMoving = true;
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

            if (partyComponent.IsLeader)
            {
                return;
            }

            if (distance >= gap)
            {
                MoveToNextTrace();
                isMoving = true;
            }
            else
            {
                if (partyComponent.Prev.GetComponent<MotorComponent>().isMoving)
                {
                    return;
                }

                if (!isMoving)
                {
                    return;
                }

                int actionHash;

                if (dirX != 0f)
                {
                    if (dirX < 0f)
                    {                        
                        actionHash = idleLeftHash;
                    }
                    else
                    {
                        actionHash = idleRightHash;
                    }
                }
                else
                {
                    if (dirY < 0f)
                    {
                        actionHash = idleDownHash;
                    }
                    else
                    {
                        actionHash = idleUpHash;
                    }
                }

                actorComponent.PlayAction(actionHash);
                isMoving = false;
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

            int actionNameHash = 0;
            if (moveType == MoveType.WalkUp || moveType == MoveType.RunUp)
            {
                actionNameHash = isRunning ? followRunUpHash : followWalkUpHash;
                dirX = 0;
                dirY = 1;
            }
            else if (moveType == MoveType.WalkDown || moveType == MoveType.RunDown)
            {
                actionNameHash = isRunning ? followRunDownHash : followWalkDownHash;
                dirX = 0;
                dirY = -1;
            }
            else if (moveType == MoveType.WalkLeft || moveType == MoveType.RunLeft)
            {
                actionNameHash = isRunning ? followRunLeftHash : followWalkLeftHash;
                dirX = -1;
                dirY = 0;
            }
            else if (moveType == MoveType.WalkRight || moveType == MoveType.RunRight)
            {             
                actionNameHash = isRunning ? followRunRightHash : followWalkRightHash;
                dirX = 1;
                dirY = 0;
            }

            actorComponent.PlayAction(actionNameHash);
            actorComponent.SetPosition(new Vector2(trace.x, trace.y));
            partyComponent.TransmitTrace(trace);

            distance -= trace.deltaDistance;
            traces.RemoveAt(0);
        }

        public void FixedUpdate(float dt)
        {
            Follow();
        }
    }
}
