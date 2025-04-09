

using Config;
using UnityEngine;

namespace Game.System
{
    public class ActorComponent
    {
        public int id;

        private Actor actor;

        public void Init(int id) 
        {
            this.id = id;
        }

        public void Refresh() 
        {
            ActorCfg cfg = Game.Config.Find<ActorCfg>(id);
            GameObject go = Game.Resource.LoadAndInstantiate(cfg.PrefabPath, Game.System.RoleSystem.Container.transform);
            actor = go.GetComponent<Actor>();
        }

        public void PlayAction() 
        {
            
        }

        protected void OnDestroy()
        {
            actor = null;
        }
    }
}