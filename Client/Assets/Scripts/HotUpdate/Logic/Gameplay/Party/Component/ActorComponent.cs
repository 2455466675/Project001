using Cysharp.Threading.Tasks;
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
        void SetVisible(bool visible);
        void SetPosition(float x, float y, float z);
        void SetPosition(Vector3 position);
        Vector3 GetPosition();
        void SetVelocity(Vector3 velocity);
        void MovePosition(Vector3 position);
    }

    /// <summary>
    /// 实体与表现层 Actor 的桥接组件。
    /// Actor 的加载时机由外部显式驱动（实体可能先于场景/Actor 就绪而创建），
    /// 在 Actor 缺席期间用影子状态承接位置与动画控制器，待加载完成后一次性回填，
    /// 从而让调用方无需感知加载是否完成。
    /// </summary>
    public class ActorComponent : ComponentBase, IActorComponent
    {
        public int ActorId { get; private set; }
        public ActorType ActorType { get; private set; }

        private Actor actor;

        // Actor 缺席期间的影子状态，加载完成后回填给 Actor
        private Vector3 position;
        private string controllerName;
        private bool visible = true;

        // 加载代际：每次加载/回收自增。异步回调据此判断结果是否已过期，
        // 防止短时间内的重复加载相互覆盖，导致先返回的 Actor 失去引用而泄漏。
        private int loadToken;

        public void SetActorId(int id)
        {
            ActorId = id;
        }

        public void SetActorType(ActorType actorType)
        {
            ActorType = actorType;
        }

        public void RefreshActor()
        {
            LoadAsync().Forget();
        }

        public void RecycleActor()
        {
            // 先让在途加载失效，避免其回调把已释放的引用重新挂回来
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

            // 加载期间又发生了新的加载/回收请求，当前结果已过期，直接释放避免泄漏
            if (token != loadToken)
            {
                Game.GetSystem<GameActorManager>().ReleaseActor(loaded);
                return;
            }

            actor = loaded;
            actor.transform.SetParent(GetParent(), false);
            ApplyState();
        }

        // Actor 就绪后一次性回填缺席期间累积的影子状态
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
