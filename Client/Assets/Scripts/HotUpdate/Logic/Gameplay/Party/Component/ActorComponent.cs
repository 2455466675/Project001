using Cysharp.Threading.Tasks;
using ECS;
using GameFramework.Core;
using UnityEngine;

namespace GameFramework.Logic
{
    public interface IActorComponent : IActor
    {
        void SetActorType(ActorType actorType);
        void SetActorId(int actorId);
        UniTask RefreshActor();
        void RecycleActor();
        void SetVisible(bool visible);
        void SetPosition(float x, float y, float z);
        void SetPosition(Vector3 position);
        Vector3 GetPosition();
        void SetVelocity(Vector3 velocity);
        void MovePosition(Vector3 position);
    }

    /// <summary>
    /// 实体与表现层 Actor 的桥接组件。
    /// </summary>
    public class ActorComponent : ComponentBase, IActorComponent
    {
        public int ActorId { get; private set; }
        public ActorType ActorType { get; private set; }

        private Actor actor;

        private Vector3 position;
        private string controllerName;
        private bool visible = true;

        private int loadToken;

        public void SetActorId(int id)
        {
            ActorId = id;
        }

        public void SetActorType(ActorType actorType)
        {
            ActorType = actorType;
        }

        public UniTask RefreshActor()
        {
             return LoadAsync();
        }

        public void RecycleActor()
        {
            loadToken++;

            if (actor == null)
            {
                return;
            }
            position = actor.GetPosition();
            Game.GetSystem<GameActorManager>().ReleaseActor(actor);
            actor = null;
        }

        public void SetVisible(bool visible)
        {
            this.visible = visible;
            if (actor == null)
            {
                return;
            }
            if (visible)
            {
                actor.gameObject.SetActive(true);
                actor.SetPosition(position);
            }
            else
            {
                position = actor.GetPosition();
                actor.gameObject.SetActive(false);
            }
        }

        public void SetPosition(float x, float y, float z)
        {
            SetPosition(new Vector3(x, y, z));
        }

        public void SetPosition(Vector3 pos)
        {
            position = pos;
            if (actor != null)
            {
                actor.SetPosition(pos);
            }
        }

        public void MovePosition(Vector3 pos)
        {
            position = pos;
            if (actor != null)
            {
                actor.MovePosition(pos);
            }
        }

        public Vector3 GetPosition()
        {
            return actor != null ? actor.GetPosition() : position;
        }

        public void SetVelocity(Vector3 velocity)
        {
            if (actor != null)
            {
                actor.SetVelocity(velocity);
            }
        }

        protected override void OnDestroy()
        {
            RecycleActor();
        }

        private async UniTask LoadAsync()
        {
            RecycleActor();
            int token = loadToken;

            Actor loaded = await Game.GetSystem<GameActorManager>().LoadActorAsync(ActorId);
            if (loaded == null)
            {
                MDebug.Error("Actor is null : ", ActorId);
                return;
            }

            if (token != loadToken)
            {
                Game.GetSystem<GameActorManager>().ReleaseActor(loaded);
                return;
            }

            actor = loaded;
            actor.transform.SetParent(GetParent(), false);
            ApplyState();
        }

        private void ApplyState()
        {
            actor.MovePosition(position);
            actor.SetPosition(position);
            if (!string.IsNullOrEmpty(controllerName))
            {
                actor.SetAnimatorController(controllerName);
            }
            actor.gameObject.SetActive(visible);
        }

        private Transform GetParent()
        {
            return GameRoot.GetNode<ActorNode>().GetActorNode(ActorType);
        }

        #region Animator

        public void SetAnimatorController(string name)
        {
            // 缓存控制器名，Actor 重载后据此恢复，避免重载丢失动画状态
            controllerName = name;
            if (actor != null)
            {
                actor.SetAnimatorController(name);
            }
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

        public float PlayAnimation(string name)
        {
            if (actor != null)
            {
                return actor.PlayAnimation(name);
            }
            else
            {
                return 0f;
            }
        }

        #endregion

        public void AddWidget(PuppetWidgetArgs args)
        {
            if (actor != null)
            {
                actor.AddWidget(args);
            }
        }
        public void RemoveWidget(string key)
        {
            if (actor != null)
            {
                actor.RemoveWidget(key);
            }
        }
    }
}
