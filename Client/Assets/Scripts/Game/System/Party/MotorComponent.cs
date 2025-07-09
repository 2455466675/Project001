using System.Collections.Generic;
using UnityEngine;

namespace Game.GSystem
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
        public float z;
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

        private bool isRunning;
        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
            private set
            {
                isRunning = value;
            }
        }

        private bool isMoving;
        public bool IsMoving
        {
            get
            {
                return isMoving;
            }
            private set
            {
                isMoving = value;
            }
        }

        /// <summary>
        /// 当前方向H
        /// </summary>
        private float dirH;
        /// <summary>
        /// 当前方向V
        /// </summary>
        private float dirV;

        /// <summary>
        /// 上一帧的位置X
        /// </summary>
        private float lastX;
        /// <summary>
        /// 上一帧的位置Z
        /// </summary>
        private float lastZ;

        private float gap;
        private int blockFrame;
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
            dirH = 0f;
            dirV = -1f;

            gap = Game.Config.Formula.GetFloatValue("PartyUnitGap");
            blockFrame = 0;
            IsRunning = false;
            traces = new List<MoveTrace>();
        }

        public void Move(float h, float v)
        {
            MoveType moveType = DoAction(h, v);
            Transmit(moveType);
            Block();
        }

        private MoveType DoAction(float h, float v)
        {
            MoveType moveType;
            int actionHash;

            if (h == 0f && v == 0f) 
            {
                if (dirH != 0f)
                {
                    if (dirH < 0f)
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
                    if (dirV < 0f)
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

                IsMoving = false;
            }
            else
            {
                if (h != 0)
                {
                    if (h > 0)
                    {
                        if (IsRunning)
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
                        if (IsRunning)
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
                    IsMoving = true;
                }
                else
                {
                    if (v > 0)
                    {
                        if (IsRunning)
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
                        if (IsRunning)
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
                dirH = h;
                dirV = v;
                IsMoving = true;
            }
            
            actorComponent.PlayAction(actionHash);
            return moveType;
        }

        private void Transmit(MoveType moveType) 
        {
            Vector3 pos = actorComponent.GetPosition();
            partyComponent.TransmitTrace(new MoveTrace() { moveType = moveType, x = pos.x, y = pos.y, z = pos.z });
        }

        /// <summary>
        /// 检测撞墙
        /// </summary>
        private void Block() 
        {
            if (!IsMoving)
            {
                return;
            }

            Vector3 pos = actorComponent.GetPosition();

            float _x = lastX;
            float _z = lastZ;

            lastX = pos.x;
            lastZ = pos.z;

            float x = lastX - _x;
            float z = lastZ - _z;
            float d = (x * x + z * z);
            if (d < 0.0001f)
            {
                blockFrame++;
                IsMoving = blockFrame < 5;
            }
            else
            {
                blockFrame = 0;
            }
        }

        public void Run(bool isRunning)
        {
            this.IsRunning = isRunning;
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
                    d = trace.z - last.z;
                }
                else if (moveType == MoveType.WalkDown || moveType == MoveType.RunDown)
                {
                    d = trace.z - last.z;
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
            IsMoving = true;
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

            if (partyComponent.IsLeader)
            {
                return;
            }

            if (distance >= gap)
            {
                MoveToNextTrace();
                IsMoving = true;
            }
            else
            {
                if (partyComponent.Prev.GetComponent<MotorComponent>().IsMoving)
                {
                    return;
                }

                if (!IsMoving)
                {
                    return;
                }

                int actionHash;

                if (dirH != 0f)
                {
                    if (dirH < 0f)
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
                    if (dirV < 0f)
                    {
                        actionHash = idleDownHash;
                    }
                    else
                    {
                        actionHash = idleUpHash;
                    }
                }

                actorComponent.PlayAction(actionHash);
                IsMoving = false;
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
                //actionNameHash = IsRunning ? followRunUpHash : followWalkUpHash;
                actionNameHash = IsRunning && moveType == MoveType.RunUp ? followRunUpHash : followWalkUpHash;
                dirH = 0;
                dirV = 1;
            }
            else if (moveType == MoveType.WalkDown || moveType == MoveType.RunDown)
            {
                //actionNameHash = IsRunning ? followRunDownHash : followWalkDownHash;
                actionNameHash = IsRunning && moveType == MoveType.RunDown ? followRunDownHash : followWalkDownHash;
                dirH = 0;
                dirV = -1;
            }
            else if (moveType == MoveType.WalkLeft || moveType == MoveType.RunLeft)
            {
                //actionNameHash = IsRunning ? followRunLeftHash : followWalkLeftHash;
                actionNameHash = IsRunning && moveType == MoveType.RunLeft ? followRunLeftHash : followWalkLeftHash;
                dirH = -1;
                dirV = 0;
            }
            else if (moveType == MoveType.WalkRight || moveType == MoveType.RunRight)
            {             
                //actionNameHash = IsRunning ? followRunRightHash : followWalkRightHash;
                actionNameHash = IsRunning && moveType == MoveType.RunRight ? followRunRightHash : followWalkRightHash;
                dirH = 1;
                dirV = 0;
            }

            actorComponent.PlayAction(actionNameHash);
            actorComponent.MovePosition(new Vector3(trace.x, trace.y, trace.z));
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
