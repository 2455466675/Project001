using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class ActorComponent : EC.Component
    {       
        public int ActorId { get; private set; }
        private Actor actor;

        public void Init(int actorId) 
        {
            ActorId = actorId;
        }

        public void RefreshActor() 
        {
            if (actor != null)
            {
                MyWorld.GetComponent<ActorFactoryComponent>().RecycleActor(actor);
                actor = null;
            }
            actor = MyWorld.GetComponent<ActorFactoryComponent>().CreateActor(ActorId, GetActorNode());
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
            if (actor == null) return Vector2.zero;
            return actor.transform.position;
        }

        public void SetPosition(Vector2 position)
        {
            if (actor == null) return;
            actor.transform.position = position;
        }

        public Transform GetBone(string boneName) 
        {
            if (actor == null) return null;
            return actor.GetBone(boneName);
        }

        public void SetColloderEnabled(bool enabled)
        {
            if (actor == null) return;
            actor.SetColloderEnabled(enabled);
        }

        protected virtual Transform GetActorNode() 
        {
            return null; 
        }

        protected override void OnDestroy()
        {
            if (actor != null)
            {
                MyWorld.GetComponent<ActorFactoryComponent>().RecycleActor(actor);
                actor = null;
            }
        }
    }
}
