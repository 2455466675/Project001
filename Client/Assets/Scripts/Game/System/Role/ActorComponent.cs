using Config;
using UnityEngine;

namespace Game.System
{
    public class ActorComponent : UnitComponent
    {
        private int id;
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

        protected override void OnDestroy()
        {
            actor = null;
        }
    }
}