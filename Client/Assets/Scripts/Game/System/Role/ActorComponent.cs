using ECS;

namespace Game.System
{
    public class ActorComponent : Entity
    {
        public int id;

        private Actor actor;

        public void Init(int id) 
        {
            this.id = id;
        }

        public void Refresh() 
        {
            
        }

        public void PlayAction() 
        {
            
        }

        protected override void OnDestroy()
        {
            actor = null;
        }
    }
}