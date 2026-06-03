using GameFramework.Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GameFramework.Logic 
{
    public class CameraNode : GameNode
    {
        [SerializeField]
        private Camera mainCamera;
        [SerializeField]
        private GameObject volume;
        [SerializeField]
        private Transform positonNode;
        [SerializeField]
        private Transform rotationNode;
        [SerializeField]
        private Transform offestNode;
        [SerializeField]
        private Transform shakeNode;

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
            if (positonNode == null)
            {
                return;
            }
            positonNode.position = position;
        }

        public void SetOffestPosition(Vector3 position)
        {
            if (offestNode == null)
            {
                return;
            }
            offestNode.localPosition = position;
        }

        public void SetRotation(Quaternion rotation)
        {
            if (rotationNode == null)
            {
                return;
            }
            rotationNode.localRotation = rotation;
        }

        public void Shake() 
        {

        }

        public void SetVolume(bool enable)
        {
            volume.SetActive(enable);
        }
    }
}