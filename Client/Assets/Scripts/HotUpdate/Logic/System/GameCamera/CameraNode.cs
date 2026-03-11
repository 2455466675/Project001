using GameFramework.Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GameFramework.Logic 
{
    public class CameraNode : GameNode
    {
        [SerializeField]
        private Camera mainCamera;

        public void AddOverlayCamera(Camera overlayCamera) 
        {
            if (mainCamera == null)
            {
                return;
            }

            var mainCameraData = mainCamera.GetUniversalAdditionalCameraData();

            var overlayCameraData = overlayCamera.GetUniversalAdditionalCameraData();
            overlayCameraData.renderType = CameraRenderType.Overlay;

            mainCameraData.cameraStack.Add(overlayCamera);
        }

        public void SetPosition(Vector3 position)
        {
            if (mainCamera == null)
            {
                return;
            }
            mainCamera.transform.position = position;
        }

        public void SetRotation(Quaternion rotation)
        {
            if (mainCamera == null)
            {
                return;
            }
            mainCamera.transform.rotation = rotation;
        }
    }
}