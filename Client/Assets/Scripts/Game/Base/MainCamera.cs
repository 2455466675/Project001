using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class MainCamera : MonoBehaviour
    {
        [SerializeField]
        private Camera m_camera;
        public Camera GameMainCamera => m_camera;
       
        private void LateUpdate()
        {
        }

        public void SetPosition(Vector3 position) 
        {
            if (m_camera == null) 
            {
                return;
            }
            m_camera.transform.position = position;
        }

        public void SetRotation(Quaternion rotation) 
        {
            if (m_camera == null)
            {
                return;
            }
            m_camera.transform.rotation = rotation;
        }
    }
}