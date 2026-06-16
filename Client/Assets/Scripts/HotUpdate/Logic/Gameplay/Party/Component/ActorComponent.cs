using ECS;
using GameFramework.Core;
using UnityEngine;

namespace GameFramework.Logic
{
    public interface IActorComponent : IAnimator
    {
        void SetActorType(ActorType actorType);
        void SetActorId(int actorId);
        void RefreshActor();
        void RecycleActor();
        void SetVisable(bool visable);
        void SetPosition(float x, float y, float z);
        void SetPosition(Vector3 position);
        Vector3 GetPosition();
        void SetVelocity(Vector3 velocity);
        void MovePosition(Vector3 position);
    }

    public class ActorComponent : ComponentBase, IActorComponent
    {
        private Actor actor;
        public int ActorId { get; private set; }
        public ActorType ActorType { get; private set; }

        private Vector3 position;

        private Vector3 ActorPosition
        {
            get
            {
                if (actor == null)
                {
                    return Vector3.zero;
                }
                else
                {
                    return actor.GetPosition();
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
                    actor.SetPosition(value);
                }
            }
        }

        public void SetActorId(int id)
        {
            ActorId = id;
        }

        public void SetActorType(ActorType actorType)
        {
            ActorType = actorType;
        }

        public void SetVelocity(Vector3 velocity)
        {
            if (actor == null)
            {
                return;
            }
            actor.SetVelocity(velocity);
        }

        public void SetPosition(float x, float y, float z)
        {
            SetPosition(new Vector3(x, y, z));
        }

        public void SetPosition(Vector3 pos)
        {
            position = pos;
            ActorPosition = pos;
        }

        public Vector3 GetPosition()
        {
            Vector3 pos = actor == null ? position : ActorPosition;
            return pos;
        }

        public void MovePosition(Vector3 pos)
        {
            if (actor == null)
            {
                return;
            }
            position = pos;
            actor.MovePosition(pos);
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
            RecycleActor();

            actor = Game.GetSystem<GameActorManager>().LoadActor(ActorId);
            if (actor == null)
            {
                MDebug.Error("Actor is null : ", ActorId);
                return;
            }

            SyncPosition();
            actor.transform.SetParent(GetParent(), false);
        }

        public async void RefreshActorAsync()
        {
            RecycleActor();

            actor = await Game.GetSystem<GameActorManager>().LoadActorAsync(ActorId);
            if (actor == null)
            {
                MDebug.Error("Actor is null : ", ActorId);
                return;
            }

            SyncPosition();
            actor.transform.SetParent(GetParent(), false);
        }

        public void RecycleActor()
        {
            if (actor == null)
            {
                return;
            }
            position = ActorPosition;
            Game.GetSystem<GameActorManager>().RecycleActor(actor);
            actor = null;
        }

        public void SetVisable(bool visable)
        {
            if (actor == null)
            {
                return;
            }
            if (visable)
            {
                actor.gameObject.SetActive(true);
                SyncPosition();
            }
            else
            {
                position = ActorPosition;
                actor.gameObject.SetActive(false);
            }
        }

        protected override void OnDestroy()
        {
            RecycleActor();
        }

        private Transform GetParent()
        {
            Transform parent = GameRoot.GetNode<ActorNode>().GetActorNode(ActorType);
            return parent;
        }

        private void SyncPosition()
        {
            MovePosition(position);
        }

        #region Animator

        public void SetAnimatorController(string name)
        {
            if (actor == null)
            {
                return;
            }
            actor.SetAnimatorController(name);
        }

        public void SetAnimatorValue(string name, bool value)
        {
            if (actor != null)
            {
                actor.SetAnimatorValue(name, value);
            }
        }

        public void SetAnimatorValue(string name, float value)
        {
            if (actor != null)
            {
                actor.SetAnimatorValue(name, value);
            }
        }

        public void SetAnimatorValue(string name, int value)
        {
            if (actor != null)
            {
                actor.SetAnimatorValue(name, value);
            }
        }

        public void SetAnimatorValue(string name)
        {
            if (actor != null)
            {
                actor.SetAnimatorValue(name);
            }
        }

        public void PlayAnimation(string name)
        {
            if (actor != null)
            {
                actor.PlayAnimation(name);
            }
        }

        #endregion
    }
}