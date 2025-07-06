using Config;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class ActorManager
    {
        private ActorRoot actorRoot;
        private Dictionary<int, Queue<Actor>> pool;

        public void Init()
        {
            pool = new Dictionary<int, Queue<Actor>>();
          
            actorRoot = Game.Root.ActorRoot;
        }

        public Transform GetScentUnitContainer() 
        {
            return actorRoot.SceneUnit;
        }

        public void SetActive(bool active) 
        {
            actorRoot.SceneUnit.gameObject.SetActive(active);
        }

        public Actor CreateActor(int id) 
        {
            Actor actor = null;

            if (pool.TryGetValue(id, out Queue<Actor> queue)) 
            {
                if (queue.Count > 0) 
                {
                    actor = queue.Dequeue();                  
                }
            }
            
            if (actor == null) 
            {
                ActorCfg cfg = Game.Config.Find<ActorCfg>(id);
                GameObject go = Game.Resource.LoadAndInstantiate(cfg.PrefabPath, actorRoot.ActorPool);
                actor = go.GetComponent<Actor>();
            }

            actor.Reuse(id);
            return actor;
        }

        public async UniTask<Actor> CreateActorAsync(int id)
        {
            Actor actor = null;

            if (pool.TryGetValue(id, out Queue<Actor> queue))
            {
                if (queue.Count > 0)
                {
                    actor = queue.Dequeue();
                }
            }

            if (actor == null)
            {
                ActorCfg cfg = Game.Config.Find<ActorCfg>(id);
                GameObject go = await Game.Resource.LoadAndInstantiateAsync(cfg.PrefabPath, actorRoot.ActorPool);
                actor = go.GetComponent<Actor>();
            }

            actor.Reuse(id);
            return actor;
        }

        public void RecycleActor(Actor actor) 
        {
            if (actor == null) 
            {
                return;
            }

            int id = actor.id;

            Queue<Actor> queue;
            if (pool.ContainsKey(id)) 
            {
                queue = pool[id];
            }
            else
            {
                queue = new Queue<Actor>();
                pool.Add(id, queue);
            }

            queue.Enqueue(actor);
            actor.Unuse();
            actor.SetParent(actorRoot.ActorPool);
            actor.transform.localPosition = Vector3.zero;
            actor.transform.localRotation = Quaternion.identity;
            actor.transform.localScale = Vector3.one;
        }
    }
}
