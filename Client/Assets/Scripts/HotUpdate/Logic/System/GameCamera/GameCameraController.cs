using GameFramework.Core;
using UnityEngine;

namespace GameFramework.Logic
{
    [GameSystem]
    public class GameCameraController : IGameSystem, IUpdateable
    {
        private Transform followTarget;

        void IUpdateable.Update(float deltaTime)
        {
            if (followTarget != null)
            {
                CameraNode node = GameRoot.GetNode<CameraNode>();
                node.SetPosition(followTarget.position);
            }
        }

        public void SetFollowTarget(Transform followTarget)
        {
            this.followTarget = followTarget;
            CameraNode node = GameRoot.GetNode<CameraNode>();
            node.SetRotation(Quaternion.Euler(15f, 0f, 0f));
        }
    }
}

