using Game.System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 角色Actor，角色的游戏对象
    /// </summary>
	public class Actor : MonoBehaviour
	{
        public int id;

        public SpriteRenderer sp;
        public Rigidbody2D rb;
        public Animator animator;
        public Collider2D _collider;
        public Bones bones;
        public ActorMotor motor;
        public ActionAssets actionAssets;
        
        public PartyRole Role { get; private set; }
        public Actor PrevActor => Role != null && Role.PrevRole != null ? Role.PrevRole.Actor : null;
        public Actor NextActor => Role != null && Role.NextRole != null ? Role.NextRole.Actor : null;
        public bool IsLeader => Role != null && Role.IsLeader;
        public bool IsMoving => motor != null && motor.IsMoving;
        public bool IsRunning => motor != null && motor.IsRunning;

        public void PlayAction(string acName)
        {
            ActionAsset ac = actionAssets.GetAction(acName);
            if (ac == null)
            {
                MLog.Error("不存在的行为:" + acName);
                return;                
            }
            ac.Execute(this);
        }

        public void SetRole(PartyRole role)
        {
            Role = role;
            SetColloderEnabled(IsLeader);
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public Vector2 GetPosition()
        {
            return transform.position;
        }

        public Transform GetBone(string name)
        {
            if (bones == null)
            {
                return transform;
            }
            else
            {
                return bones.GetBone(name);
            }
        }

        public void SetColloderEnabled(bool enabled)
        {
            if (_collider == null) return;
            _collider.enabled = enabled;
        }

        public void Move(Vector2 dir) 
        {
            if (motor == null)
            {
                return;
            }
            motor.Move(dir);
        }

        public void Run(bool isRunning)
        {
            if (motor == null)
            {
                return;
            }
            motor.IsRunning = isRunning;
        }

        /// <summary>
        /// 将足迹传递给下一个角色
        /// </summary>
        /// <param name="trace"></param>
        /// <returns>是否发生移动</returns>
        public bool AddNextRoleTrace(MoveTrace trace)
        {
            if (NextActor == null)
            {
                return true;
            }
            return NextActor.motor.AddTrace(trace);
        }

        [Button("Init")]
        private void Init()
        {
            sp = GetComponentInChildren<SpriteRenderer>();
            rb = GetComponentInChildren<Rigidbody2D>();
            animator = GetComponentInChildren<Animator>();
            _collider = GetComponentInChildren<Collider2D>();
            bones = GetComponentInChildren<Bones>();
            motor = GetComponentInChildren<ActorMotor>();
            actionAssets = GetComponentInChildren<ActionAssets>();
        }
    }
}

