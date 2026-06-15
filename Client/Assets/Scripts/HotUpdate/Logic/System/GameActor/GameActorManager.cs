using Config;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Core;
using Cysharp.Threading.Tasks;

namespace GameFramework.Logic
{
    [GameSystem]
    public class GameActorManager : IGameSystem, IInit
    {
        private Dictionary<int, Queue<Actor>> actorPool;

        public void Init()
        {
            actorPool = new Dictionary<int, Queue<Actor>>();
        }

        public Actor LoadActor(int id)
        {
            Actor actor;

            Queue<Actor> pool = GetPool(id);
            if (pool.Count == 0)
            {
                ActorCfg cfg = Game.Config.Find<ActorCfg>(id);
                if (cfg == null)
                {
                    MDebug.Error("ActorCfg is null : ", id);
                    return null;
                }
                else
                {
                    GameObject obj = Game.Resources.LoadAndInstantiate(cfg.PrefabPath, null);
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
                ActorCfg cfg = Game.Config.Find<ActorCfg>(id);
                if (cfg == null)
                {
                    MDebug.Error("ActorCfg is null : ", id);
                    return null;
                }
                else
                {
                    GameObject obj = await Game.Resources.LoadAndInstantiateAsync(cfg.PrefabPath, null);
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
            actor.transform.localPosition = Vector3.zero;

            pool.Enqueue(actor);
        }

        private Queue<Actor> GetPool(int id)
        {
            if (actorPool.ContainsKey(id))
            {
                return actorPool[id];
            }
            else
            {
                Queue<Actor> pool = new Queue<Actor>();
                actorPool[id] = pool;
                return pool;
            }
        }
    }
}


