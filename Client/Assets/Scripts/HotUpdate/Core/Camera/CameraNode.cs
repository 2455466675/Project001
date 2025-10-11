using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GameFramework.Core 
{
    public class CameraNode : GameNode
    {
        [SerializeField]
        private Camera m_Camera;

        public void AddOverlayCamera(Camera overlayCamera) 
        {
            if (m_Camera == null)
            {
                return;
            }

            var mainCameraData = m_Camera.GetUniversalAdditionalCameraData();

            var overlayCameraData = overlayCamera.GetUniversalAdditionalCameraData();
            overlayCameraData.renderType = CameraRenderType.Overlay;

            mainCameraData.cameraStack.Add(overlayCamera);
        }

        public void SetPosition(Vector3 position)
        {
            if (m_Camera == null)
            {
                return;
            }
            m_Camera.transform.position = position;
        }

        public void SetRotation(Quaternion rotation)
        {
            if (m_Camera == null)
            {
                return;
            }
            m_Camera.transform.rotation = rotation;
        }
    }
}