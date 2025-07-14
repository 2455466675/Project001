using Game.GSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    public class MainCamera : MonoBehaviour
    {
        [SerializeField]
        private Camera m_camera;
        public Camera GameMainCamera => m_camera;
       
        private void LateUpdate()
        {
            if (Game.IsInited && Game.State.IsPlaying) 
            {
                if (Game.System.BattleSystem.IsBattle) 
                {
                    //m_camera.transform.position = new Vector3(6, 6, 0);
                    //m_camera.transform.rotation = Quaternion.AngleAxis(70f, Vector3.right);
                }
                else
                {
                    var leader = Game.System.PartySystem.Leader;
                    if (leader != null)
                    {
                        Vector3 pos = leader.GetComponent<ActorComponent>().GetPosition();
                        SetPosition(new Vector3(pos.x, 2, pos.z - 7));
                    }
                }                
            }
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