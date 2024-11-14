using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class Bones : MonoBehaviour
	{
        public List<Transform> bones;

        public Transform GetBone(string boneName)
        {
            if (bones == null || bones.Count <= 0) return transform;
            Transform t = bones.Find(x => x.transform.name == boneName);
            return t != null ? t : transform;
        }

        [Button("Init")]
        private void Init()
        {
            string[] bonesArray = SystemSetting.Bones();
            if (bonesArray == null || bonesArray.Length == 0)
            {
                return;
            }
            
            bones = new List<Transform>();

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (bonesArray.Contains(child.name))
                {
                    bones.Add(child);
                } 
            }
        }
    }
}

