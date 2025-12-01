using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GameFramework.Core 
{
    public class CameraNode : GameNode
    {
        [SerializeField]
        private Transform m_CameraPoint;

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

        public Vector3 GetPositon()
        {
            if (m_CameraPoint == null)
            {
                return Vector3.zero;
            }
            return m_CameraPoint.position; 
        }

        public void SetPosition(Vector3 position)
        {
            if (m_CameraPoint == null)
            {
                return;
            }
            m_CameraPoint.position = position;
        }

        public Quaternion GetRotation()
        {
            if (m_CameraPoint == null)
            {
                return Quaternion.identity;
            }
            return m_CameraPoint.rotation;
        }

        public void SetRotation(Quaternion rotation)
        {
            if (m_CameraPoint == null)
            {
                return;
            }
            m_CameraPoint.rotation = rotation;
        }
    }
}