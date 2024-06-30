using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
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
	}
}

