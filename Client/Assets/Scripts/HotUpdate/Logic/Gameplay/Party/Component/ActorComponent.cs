using ECS;
using UnityEngine;
using GameFramework.Core;

namespace GameFramework.Logic
{
    public class ActorComponent : ComponentBase, IActor
    {
        private Actor actor;
        public int ActorId { get; private set; }
        public ActorType ActorType { get; private set; }

        public float DirX { get; private set; }
        public float DirY { get; private set; }

        #region IActor

        public Transform Transform => actor != null ? actor.Transform : null;

        public Vector3 Velocity
        {
            get
            {
                if (actor == null)
                {
                    return Vector3.zero;
                }
                else
                {
                    return actor.Velocity;
                }
            }
            set
            {
                if (actor == null)
                {
                    return;
                }
                else
                {
                    actor.Velocity = value;
                }
            }
        }

        public Vector3 Position
        {
            get
            {
                if (actor == null)
                {
                    return Vector3.zero;
                }
                else
                {
                    return actor.Position;
                }
            }
            set
            {
                if (actor == null)
                {
                    return;
                }
                else
                {
                    actor.Position = value;
                }
            }
        }

        public Quaternion Rotation
        {
            get
            {
                if (actor == null)
                {
                    return Quaternion.identity;
                }
                else
                {
                    return actor.Rotation;
                }
            }
            set
            {
                if (actor == null)
                {
                    return;
                }
                else
                {
                    actor.Rotation = value;
                }
            }
        }

        public Vector3 LocalPosition
        {
            get
            {
                if (actor == null)
                {
                    return Vector3.zero;
                }
                else
                {
                    return actor.LocalPosition;
                }
            }
            set
            {
                if (actor == null)
                {
                    return;
                }
                else
                {
                    actor.LocalPosition = value;
                }
            }
        }

        public void MovePosition(Vector3 pos)
        {
            if (actor == null)
            {
                return;
            }
            actor.MovePosition(pos);
        }

        public void SetKinematic(bool isKinematic)
        {
            if (actor == null)
            {
                return;
            }
            actor.SetKinematic(isKinematic);
        }

        public void SetAnimatorController(string name)
        {
            if (actor == null)
            {
                return;
            }
            actor.SetAnimatorController(name);
        }

        public void SetBool(string name, bool value)
        {
            if (actor == null)
            {
                return;
            }
            actor.SetBool(name, value);
        }

        public void SetFloat(string name, float value)
        {
            if (actor == null)
            {
                return;
            }
            actor.SetFloat(name, value);
        }

        public void SetInteger(string name, int value)
        {
            if (actor == null)
            {
                return;
            }
            actor.SetInteger(name, value);
        }

        public void SetTrigger(string name)
        {
            if (actor == null)
            {
                return;
            }
            actor.SetTrigger(name);
        }

        public void PlayAnim(string name)
        {
            if (actor == null)
            {
                return;
            }
            actor.PlayAnim(name);
        }

        #endregion

        public void SetActorId(int id)
        {
            ActorId = id;
        }

        public void SetActorType(ActorType actorType)
        {
            ActorType = actorType;
        }

        public void SetDirection(float dirX, float dirY)
        {
            if (DirX != dirX || DirY != dirY)
            {
                SetFloat("DirX", dirX);
                SetFloat("DirY", dirY);
            }

            DirX = dirX;
            DirY = dirY;
        }

        public Transform GetBone(string name)
        {
            if (actor == null)
            {
                return null;
            }
            return actor.GetBone(name);
        }

        public void RefreshActor()
        {
            ClearActor();

            actor = Game.GetSystem<GameActorManager>().LoadActor(ActorId);
            if (actor == null)
            {
                MDebug.Error("Actor is null : ", ActorId);
                return;
            }

            Transform parent = GetParent();
            actor.transform.SetParent(parent, false);
        }

        public async void RefreshActorAsync()
        {
            ClearActor();

            actor = await Game.GetSystem<GameActorManager>().LoadActorAsync(ActorId);
            if (actor == null)
            {
                MDebug.Error("Actor is null : ", ActorId);
                return;
            }

            Transform parent = GetParent();
            actor.transform.SetParent(parent, false);
        }

        private Transform GetParent()
        {
            Transform parent = GameRoot.GetNode<ActorNode>().GetActorNode(ActorType);
            return parent;
        }

        protected override void OnDestroy()
        {
            ClearActor();
        }

        private void ClearActor()
        {
            if (actor == null)
            {
                return;
            }
            Game.GetSystem<GameActorManager>().RecycleActor(actor);
            actor = null;
        }
    }
}