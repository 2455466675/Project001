using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Game.GSystem
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
            //await UniTask.WaitForSeconds(GameMathf.Random(1, 10));

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

        public ActionHandle PlayAction(int hash, object userData = null) 
        {
            if (actor == null)
            {
                return null;
            }
            return actor.PlayAction(hash, userData);
        }

        public Vector3 GetPosition() 
        {
            if (actor == null)
            {
                return Vector3.zero;
            }
            return actor.Rigidbody.position;
        }

        public void MovePosition(Vector3 pos) 
        {
            if (actor == null)
            {
                return;
            }
            actor.Rigidbody.MovePosition(pos);
        }

        public void SetLocalPosition(Vector3 pos) 
        {
            if (actor == null)
            {
                return;
            }
            actor.transform.localPosition = pos;
        }

        public void SetLocalRotation(float x) 
        {
            if (actor == null)
            {
                return;
            }
            actor.transform.localRotation = Quaternion.AngleAxis(x, Vector3.right);
        }

        public void SetRigidbodyEnable(bool active) 
        {
            if (actor == null)
            {
                return;
            }
            actor.boxCollider.enabled = active;
            actor.Rigidbody.isKinematic = !active;
        }

        protected abstract Transform GetActorNode();

        protected override void OnDestroyComponent()
        {
            RecycleActor();
            OnRefreshActorEvent = null;
        }
    }
}