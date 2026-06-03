using UnityEngine;

namespace GameFramework.Logic
{
    public interface IActorComponent
    {
        void SetActorType(ActorType actorType);
        void SetActorId(int actorId);
        void RefreshActor();
        void RecycleActor();
        void SetVisable(bool visable);
        void SetPosition(float x, float y, float z);
        (float x, float y, float z) GetPosition();
    }

    public class ActorComponent : ProjectableComponent<IActorComponent>, IActorComponent
    {
        public void SetActorId(int actorId)
        {
            projection.SetActorId(actorId);
        }

        public void SetActorType(ActorType actorType)
        {
            projection.SetActorType(actorType);
        }

        public void SetPosition(float x, float y, float z)
        {
            projection.SetPosition(x, y, z);
        }

        public (float x, float y, float z) GetPosition()
        {
            return projection.GetPosition();
        }

        public void RefreshActor()
        {
            projection.RefreshActor();
        }

        public void RecycleActor()
        {
            projection.RecycleActor();
        }

        public void SetVisable(bool visable)
        {
            projection.SetVisable(visable);
        }
    }
}