using Cysharp.Threading.Tasks;
using ECS;
using GameFramework.Core;
using GameFramework.Logic;
using PlasticGui.WorkspaceWindow.Home;
using UnityEngine;
using UnityEngine.UIElements;
using static Codice.Client.Commands.WkTree.WorkspaceTreeNode;

namespace GameFramework.View
{
    public class ActorProjectionComponent : ComponentBase, IActor, IProjection, IActorComponent
    {
        private Actor actor;
        public int ActorId { get; private set; }
        public ActorType ActorType { get; private set; }
        public float DirX { get; private set; }
        public float DirY { get; private set; }

        private Vector3 position;

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
            position = Position;
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
                position = Position;
                actor.gameObject.SetActive(false);
            }
        }

        public void SetPosition(float x, float y, float z)
        {
            position = new Vector3(x, y, z);
            Position = position;
        }

        public (float x, float y, float z) GetPosition()
        {
            Vector3 pos = actor == null ? position : Position;
            return (pos.x, pos.y, pos.z);
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
            SetFloat("DirX", DirX);
            SetFloat("DirY", DirY);
        }
    }
}
