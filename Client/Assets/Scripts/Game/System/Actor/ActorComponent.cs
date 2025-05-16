using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Game.System
{
    public abstract class ActorComponent : UnitComponent
    {
        protected int id;
        protected Actor actor;

        public event Action<Actor> OnRefreshActorEvent;

        public void Init(int id) 
        {
            this.id = id;
        }

        public void RefreshActor() 
        {            
            if (actor != null) 
            {
                if (actor.id == id) 
                {
                    return;
                }
                else
                {
                    RecycleActor();
                }                
            }

            actor = Game.System.ActorManager.CreateActor(id);
            actor.SetParent(GetActorNode());
            actor.transform.localPosition = Vector3.zero;
            actor.transform.localRotation = Quaternion.identity;
            actor.transform.localScale = Vector3.one;
            OnRefreshActorEvent?.Invoke(actor);
        }

        public async UniTask RefreshActorAsync()
        {
            if (actor != null)
            {
                if (actor.id == id)
                {
                    return;
                }
                else
                {
                    RecycleActor();
                }
            }

            MLog.Log("a-RefreshActorAsync", id);
            await UniTask.WaitForSeconds(GameMathf.Random(1, 10));

            actor = await Game.System.ActorManager.CreateActorAsync(id);

            MLog.Log("b-RefreshActorAsync", id);
            actor.SetParent(GetActorNode());
            actor.transform.localPosition = Vector3.zero;
            actor.transform.localRotation = Quaternion.identity;
            actor.transform.localScale = Vector3.one;
            OnRefreshActorEvent?.Invoke(actor);
        }

        public void RecycleActor() 
        {
            if (actor == null) 
            {
                return;
            }

            Game.System.ActorManager.RecycleActor(actor);
            actor = null;
        }

        public void PlayAction(int hash, object userData = null) 
        {
            actor.PlayAction(hash, userData);
        }

        public Vector2 GetPosition() 
        {
            return actor.Rigidbody2D.position;
        }

        public void SetPosition(Vector2 pos) 
        {
            actor.Rigidbody2D.MovePosition(pos);
        }

        public void SetRigidbodyEnable(bool active) 
        {
            actor.boxCollider2D.enabled = active;
            actor.Rigidbody2D.bodyType = active ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
        }

        protected abstract Transform GetActorNode();

        protected override void OnDestroyComponent()
        {
            RecycleActor();
            OnRefreshActorEvent = null;
        }
    }
}