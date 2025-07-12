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
        private static readonly int velocityWalkUpHash = Common.StringToHash("VelocityWalkUp");
        private static readonly int velocityWalkDownHash = Common.StringToHash("VelocityWalkDown");
        private static readonly int velocityWalkLeftHash = Common.StringToHash("VelocityWalkLeft");
        private static readonly int velocityWalkRightHash = Common.StringToHash("VelocityWalkRight");
        private static readonly int velocityRunUpHash = Common.StringToHash("VelocityRunUp");
        private static readonly int velocityRunDownHash = Common.StringToHash("VelocityRunDown");
        private static readonly int velocityRunLeftHash = Common.StringToHash("VelocityRunLeft");
        private static readonly int velocityRunRightHash = Common.StringToHash("VelocityRunRight");
        private static readonly int velocityIdleHash = Common.StringToHash("VelocityIdle");

        private static readonly int animatorIdleUpHash = Common.StringToHash("AnimatorIdleUp");
        private static readonly int animatorIdleDownHash = Common.StringToHash("AnimatorIdleDown");
        private static readonly int animatorIdleLeftHash = Common.StringToHash("AnimatorIdleLeft");
        private static readonly int animatorIdleRightHash = Common.StringToHash("AnimatorIdleRight");
        private static readonly int animatorWalkUpHash = Common.StringToHash("AnimatorWalkUp");
        private static readonly int animatorWalkDownHash = Common.StringToHash("AnimatorWalkDown");
        private static readonly int animatorWalkLeftHash = Common.StringToHash("AnimatorWalkLeft");
        private static readonly int animatorWalkRightHash = Common.StringToHash("AnimatorWalkRight");
        private static readonly int animatorRunUpHash = Common.StringToHash("AnimatorRunUp");
        private static readonly int animatorRunDownHash = Common.StringToHash("AnimatorRunDown");
        private static readonly int animatorRunLeftHash = Common.StringToHash("AnimatorRunLeft");
        private static readonly int animatorRunRightHash = Common.StringToHash("AnimatorRunRight");

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
            int actionHash_1;
            int actionHash_2;

            if (h == 0f && v == 0f) 
            {
                if (dirH != 0f)
                {
                    if (dirH < 0f)
                    {
                        actionHash_1 = animatorIdleLeftHash;
                        actionHash_2 = velocityIdleHash;
                        moveType = MoveType.IdleLeft;
                    }
                    else
                    {
                        actionHash_1 = animatorIdleRightHash;
                        actionHash_2 = velocityIdleHash;
                        moveType = MoveType.IdleRight;
                    }
                }
                else
                {
                    if (dirV < 0f)
                    {
                        actionHash_1 = animatorIdleDownHash;
                        actionHash_2 = velocityIdleHash;
                        moveType = MoveType.IdleDown;
                    }
                    else
                    {
                        actionHash_1 = animatorIdleUpHash;
                        actionHash_2 = velocityIdleHash;
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
                            actionHash_1 = animatorRunRightHash;
                            actionHash_2 = velocityRunRightHash;
                            moveType = MoveType.RunRight;
                        }
                        else
                        {
                            actionHash_1 = animatorWalkRightHash;
                            actionHash_2 = velocityWalkRightHash;
                            moveType = MoveType.WalkRight;
                        }
                    }
                    else
                    {
                        if (IsRunning)
                        {
                            actionHash_1 = animatorRunLeftHash;
                            actionHash_2 = velocityRunLeftHash;
                            moveType = MoveType.RunLeft;
                        }
                        else
                        {
                            actionHash_1 = animatorWalkLeftHash;
                            actionHash_2 = velocityWalkLeftHash;
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
                            actionHash_1 = animatorRunUpHash;
                            actionHash_2 = velocityRunUpHash;
                            moveType = MoveType.RunUp;
                        }
                        else
                        {
                            actionHash_1 = animatorWalkUpHash;
                            actionHash_2 = velocityWalkUpHash;
                            moveType = MoveType.WalkUp;
                        }
                    }
                    else
                    {
                        if (IsRunning)
                        {
                            actionHash_1 = animatorRunDownHash;
                            actionHash_2 = velocityRunDownHash;
                            moveType = MoveType.RunDown;
                        }
                        else
                        {
                            actionHash_1 = animatorWalkDownHash;
                            actionHash_2 = velocityWalkDownHash;
                            moveType = MoveType.WalkDown;
                        }
                    }
                }
                dirH = h;
                dirV = v;
                IsMoving = true;
            }
            
            actorComponent.PlayAction(actionHash_1);
            actorComponent.PlayAction(actionHash_2);
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
                        actionHash = animatorIdleLeftHash;
                    }
                    else
                    {
                        actionHash = animatorIdleRightHash;
                    }
                }
                else
                {
                    if (dirV < 0f)
                    {
                        actionHash = animatorIdleDownHash;
                    }
                    else
                    {
                        actionHash = animatorIdleUpHash;
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
                actionNameHash = IsRunning && moveType == MoveType.RunUp ? animatorRunUpHash : animatorWalkUpHash;
                dirH = 0;
                dirV = 1;
            }
            else if (moveType == MoveType.WalkDown || moveType == MoveType.RunDown)
            {
                //actionNameHash = IsRunning ? followRunDownHash : followWalkDownHash;
                actionNameHash = IsRunning && moveType == MoveType.RunDown ? animatorRunDownHash : animatorWalkDownHash;
                dirH = 0;
                dirV = -1;
            }
            else if (moveType == MoveType.WalkLeft || moveType == MoveType.RunLeft)
            {
                //actionNameHash = IsRunning ? followRunLeftHash : followWalkLeftHash;
                actionNameHash = IsRunning && moveType == MoveType.RunLeft ? animatorRunLeftHash : animatorWalkLeftHash;
                dirH = -1;
                dirV = 0;
            }
            else if (moveType == MoveType.WalkRight || moveType == MoveType.RunRight)
            {             
                //actionNameHash = IsRunning ? followRunRightHash : followWalkRightHash;
                actionNameHash = IsRunning && moveType == MoveType.RunRight ? animatorRunRightHash : animatorWalkRightHash;
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
