using UnityEngine;

namespace GameFramework.Logic
{
    internal class ControlledCameraModel : BaseCameraModel
    {
        public override CameraModel CameraModel => CameraModel.Controlled;

        public override void Enter()
        {
            cameraNode.SetRotation(Quaternion.Euler(0f, 0f, 0f));
            cameraNode.SetPosition(new Vector3(0f, 1f, -6f));
            cameraNode.SetOffestPosition(new Vector3(0f, 0f, 0f));
            cameraNode.SetVolume(false);
        }

        public override void Exit()
        {
            
        }

        public override void Update()
        {
            
        }
    }
}
