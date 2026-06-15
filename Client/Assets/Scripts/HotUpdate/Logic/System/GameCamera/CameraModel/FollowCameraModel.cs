using UnityEngine;

namespace GameFramework.Logic
{
    internal class FollowCameraModel : BaseCameraModel
    {
        public override CameraModel CameraModel => CameraModel.Follow;

        private ActorComponent actorComponent;

        public override void Enter()
        {
            cameraNode.SetRotation(Quaternion.Euler(10f, 0f, 0f));
            cameraNode.SetOffestPosition(new Vector3(0f, 0.5f, -7f));
            cameraNode.SetVolume(true);
            ChickActorComponent();
        }

        public override void Exit()
        {
            actorComponent = null;
        }

        public override void Update()
        {
            ChickActorComponent();
            SyncPosition();
        }

        private void ChickActorComponent()
        {
            if (actorComponent != null)
            {
                return;
            }
            var leader = Game.GetModule<PartyModule>().Leader;
            if (leader == null)
            {
                return;
            }
            actorComponent = leader.GetComponent<ActorComponent>();
        }

        private void SyncPosition()
        {
            if (actorComponent == null)
            {
                return;
            }
            cameraNode.SetPosition(actorComponent.GetPosition());
        }
    }
}