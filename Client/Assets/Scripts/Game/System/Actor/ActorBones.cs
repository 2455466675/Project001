using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class ActorBones : MonoBehaviour
    {
        [SerializeField]
        private List<Transform> bones;

        public Transform GetBone(string boneName) 
        {
            if (bones == null) return null;

            return bones.Find(t => t.name == boneName);
        }

        [Button("Init")]
        private void Init() 
        {
            bones = new List<Transform>();
            for (int i = 0; i < transform.childCount; i++) 
            { 
                Transform child = transform.GetChild(i);
                
                if (bones.Find(t => t.name == child.name) != null) 
                {
                    continue;
                }   

                bones.Add(child);
            }
        }
    }
}
