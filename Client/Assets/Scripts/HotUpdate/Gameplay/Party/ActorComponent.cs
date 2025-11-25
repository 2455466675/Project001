using UnityEngine;
using GameFramework.Core;

namespace GameFramework.Gameplay
{
    public class ActorComponent : Featrue.Component
    {
        private Actor m_Actor;
        public int ActorId { get; set; }
        public ActorType ActorType { get; set; }
        public Transform Transform => m_Actor != null ? m_Actor.transform : null;

        public Vector3 Velocity
        {
            get
            {
                if (m_Actor == null)
                {
                    return Vector3.zero;
                }
                else
                {
                    return m_Actor.Velocity;
                }
            }
            set
            {
                if (m_Actor == null)
                {
                    return;
                }
                else
                {
                    m_Actor.Velocity = value;
                }
            }
        }

        public Vector3 Position
        {
            get
            {
                if (m_Actor == null)
                {
                    return Vector3.zero;
                }
                else
                {
                    return m_Actor.Position;
                }
            }
            set
            {
                if (m_Actor == null)
                {
                    return;
                }
                else
                {
                    m_Actor.Position = value;
                }
            }
        }

        public void MovePosition(Vector3 pos)
        {
            if (m_Actor == null)
            {
                return;
            }
            m_Actor.MovePosition(pos);
        }

        public void SetKinematic(bool isKinematic)
        {
            if (m_Actor == null)
            {
                return;
            }
            m_Actor.SetKinematic(isKinematic);
        }

        public void RefreshActor()
        {
            ActorManager manager = Game.GetModule<ActorManager>();

            if (m_Actor != null)
            {
                manager.RecycleActor(m_Actor);
                m_Actor = null;
            }

            m_Actor = manager.LoadActor(ActorId);

            if (m_Actor == null)
            {
                MDebug.Warn("Actor is null : ", ActorId);
                return;
            }

            Transform parent = GetParent();
            m_Actor.transform.SetParent(parent, false);
        }

        public async void RefreshActorAsync()
        {
            ActorManager manager = Game.GetModule<ActorManager>();

            if (m_Actor != null)
            {
                manager.RecycleActor(m_Actor);
                m_Actor = null;
            }

            m_Actor = await manager.LoadActorAsync(ActorId);

            if (m_Actor == null)
            {
                MDebug.Warn("Actor is null : ", ActorId);
                return;
            }

            Transform parent = GetParent();
            m_Actor.transform.SetParent(parent, false);
        }

        #region Animator

        public void SetAnimatorController(string name)
        {
            if (m_Actor == null || m_Actor.Animator == null)
            {
                return;
            }
            m_Actor.Animator.SetAnimatorController(name);
        }

        public void SetBool(string name, bool value)
        {
            if (m_Actor == null || m_Actor.Animator == null)
            {
                return;
            }
            m_Actor.Animator.SetBool(name, value);
        }

        public void SetFloat(string name, float value)
        {
            if (m_Actor == null || m_Actor.Animator == null)
            {
                return;
            }
            m_Actor.Animator.SetFloat(name, value);
        }

        public void SetInteger(string name, int value)
        {
            if (m_Actor == null || m_Actor.Animator == null)
            {
                return;
            }
            m_Actor.Animator.SetInteger(name, value);
        }

        public void SetTrigger(string name)
        {
            if (m_Actor == null || m_Actor.Animator == null)
            {
                return;
            }
            m_Actor.Animator.SetTrigger(name);
        }

        #endregion     

        private Transform GetParent()
        {
            Transform parent = GameRoot.GetNode<ActorNode>().GetActorNode(ActorType);
            return parent;
        }

        protected override void OnDestroy()
        {
            if (m_Actor == null)
            {
                return;
            }
            Game.GetModule<ActorManager>().RecycleActor(m_Actor);
            m_Actor = null;
        }
    }
}
