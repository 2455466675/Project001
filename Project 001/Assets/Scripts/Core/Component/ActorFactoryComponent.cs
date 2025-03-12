using EC;
using Game.Cfg;
using Game.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class ActorFactoryComponent : EC.Component, IAwake
    {
        private Dictionary<int, Actor> _pool;

        private Transform poolNode;

        public void Awake()
        {
            _pool = new Dictionary<int, Actor>();
        }

        public Actor CreateActor(int actorId, Transform node) 
        {
            if (_pool.ContainsKey(actorId)) 
            {
                Actor actor = _pool[actorId];
                actor.gameObject.SetActive(true);
                actor.transform.SetParent(node);
                actor.transform.localPosition = Vector3.zero;
                actor.transform.localRotation = Quaternion.identity;
                actor.transform.localScale = Vector3.one;
                return actor;
            }
            else
            {
                ConfigComponent cc = World.GetComponent<ConfigComponent>();
                ActorCfg actorCfg = cc.Find<ActorCfg>(actorId);
                if (actorCfg == null)
                {
                    MLog.Log("actorCfg == null", actorId);
                    return null;
                }

                GameObject obj = World.GetComponent<ResourceComponent>().LoadAndInstantiate(actorCfg.PrefabPath, node);
                Actor actor = obj.GetComponent<Actor>();
                actor.id = actorId;
                return actor;
            }
        }

        public void RecycleActor(Actor actor) 
        {            
            if (actor == null)
            {
                return;
            }

            int actorId = actor.id;
            if (_pool.ContainsKey(actorId)) 
            {
                return;
            }

            if (poolNode == null) 
            {
                poolNode = GameObject.FindWithTag("ActorPool").transform;
            }

            actor.transform.SetParent(poolNode.transform, false);
            actor.transform.localPosition = Vector3.zero;
            actor.transform.localRotation = Quaternion.identity;
            actor.transform.localScale = Vector3.one;
            actor.gameObject.SetActive(false);

            _pool.Add(actorId, actor);
        }
    }
}
