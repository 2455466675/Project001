using Game.GSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MainCamera : MonoBehaviour
    {
        [SerializeField]
        private Camera m_camera;

        private void LateUpdate()
        {
            if (Game.IsInited && Game.State.IsPlaying) 
            {
                var leader = Game.System.PartySystem.Leader;
                if (leader != null)
                {
                    Vector3 pos = leader.GetComponent<ActorComponent>().GetPosition();
                    Vector3 originPos = m_camera.transform.position;
                    m_camera.transform.position = new Vector3(pos.x, originPos.y, pos.z - 7);
                }
            }
        }
    }
}