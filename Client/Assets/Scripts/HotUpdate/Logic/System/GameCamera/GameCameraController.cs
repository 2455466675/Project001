using GameFramework.Core;
using System.Collections.Generic;

namespace GameFramework.Logic
{
    [GameSystem]
    public class GameCameraController : IGameSystem, ILateUpdateable
    {
        private BaseCameraModel currentModel;
        private Dictionary<CameraModel, BaseCameraModel> cameraModels;

        void ILateUpdateable.LateUpdate(float deltaTime)
        {
            currentModel?.Update();            
        }

        public void Start()
        {
            var cameraNode = GameRoot.GetNode<CameraNode>();
            BaseCameraModel model1 = new FollowCameraModel();
            BaseCameraModel model2 = new ControlledCameraModel();
            model1.Init(cameraNode);
            model2.Init(cameraNode);

            cameraModels = new Dictionary<CameraModel, BaseCameraModel>();
            cameraModels[CameraModel.Follow] = model1;
            cameraModels[CameraModel.Controlled] = model2;
            currentModel = null;
        }

        public void SetCameraModel(CameraModel cameraModel)
        {
            if (currentModel != null && currentModel.CameraModel == cameraModel)
            {
                return;
            }

            currentModel?.Exit();
            currentModel = cameraModels[cameraModel];
            currentModel?.Enter();
        }
    }
}

