using Config;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Core
{
    [GameModule]
    public class ActorManager : IGameModule_SyncInit
    {
        private Dictionary<int, Queue<Actor>> m_ActorPool;

        public void Init()
        {
            m_ActorPool = new Dictionary<int, Queue<Actor>>();
        }

        public Actor LoadActor(int id)
        {
            Actor actor;

            Queue<Actor> pool = GetPool(id);
            if (pool.Count == 0)
            {
                ActorCfg cfg = Game.GetModule<ConfigManager>().Find<ActorCfg>(id);
                if (cfg == null)
                {
                    actor = null;
                }
                else
                {
                    GameObject obj = Game.GetModule<AssetsManager>().LoadAndInstantiate(cfg.PrefabPath, null);
                    actor = obj.GetComponent<Actor>();
                    actor.Id = id;               
                }
            }
            else
            {
                actor = pool.Dequeue();
            }

            actor.transform.SetParent(null);
            return actor;
        }

        public async UniTask<Actor> LoadActorAsync(int id)
        {
            Actor actor;
            Queue<Actor> pool = GetPool(id);
            if (pool.Count == 0)
            {
                ActorCfg cfg = Game.GetModule<ConfigManager>().Find<ActorCfg>(id);
                if (cfg == null)
                {
                    actor = null;
                }
                else
                {
                    GameObject obj = await Game.GetModule<AssetsManager>().LoadAndInstantiateAsync(cfg.PrefabPath, null);
                    actor = obj.GetComponent<Actor>();
                    actor.Id = id;                   
                }
            }
            else
            {
                actor = pool.Dequeue();
            }
            actor.transform.SetParent(null);
            return actor;
        }

        public void RecycleActor(Actor actor)
        {
            int id = actor.Id;
            Queue<Actor> pool = GetPool(id);

            Transform parent = GameRoot.GetNode<ActorNode>().GetActorNode(ActorType.Pool);
            actor.transform.SetParent(null);
            actor.transform.SetParent(parent);
            actor.LocalPosition = Vector3.zero;
            
            pool.Enqueue(actor);
        }

        private Queue<Actor> GetPool(int id)
        {
            if (m_ActorPool.ContainsKey(id))
            {
                return m_ActorPool[id];
            }
            else
            {
                Queue<Actor> pool = new Queue<Actor>();
                m_ActorPool[id] = pool;
                return pool;
            }
        }
    }
}
