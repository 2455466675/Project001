using Game.Cfg;
using Game.Core;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class ActorComponent : EC.Component
    {       
        private Actor actor;

        public void Init(int id) 
        {
            ConfigComponent cc = MyWorld.GetComponent<ConfigComponent>();
            RoleCfg Cfg = cc.Find<RoleCfg>(id);
            if (Cfg == null)
            {
                return;
            }

            ActorCfg actorCfg = cc.Find<ActorCfg>(Cfg.Actor);
            if (actorCfg == null) 
            {
                return;
            }

            GameObject obj = MyWorld.GetComponent<ResourceComponent>().LoadAndInstantiate(actorCfg.PrefabPath, GameObject.FindWithTag("CharacterContainer").transform);
            actor = obj.GetComponent<Actor>();
        }

        public void PlayAction(string actionName, params object[] actionArgs) 
        {
            if (actor == null) 
            {
                return;
            }
            ActionGroup action = MyWorld.GetComponent<SystemComponent>().Config.FindAction(actionName);
            if (action == null) 
            {
                return;
            }
            action.Execute(actor, actionArgs);
        }

        public Vector2 GetPosition() 
        {
            return actor.transform.position;
        }

        public void SetPosition(Vector2 position)
        {
            actor.transform.position = position;
        }

        public void SetColloderEnabled(bool enabled)
        {
            if (actor == null) return;
            actor.SetColloderEnabled(enabled);
        }
    }
}
