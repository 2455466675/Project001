using System;
using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
	public class SceneMap : MonoBehaviour
	{
        public string sceneName;

        public Camera sceneCamera;

        public SceneContainer[] containers;

        private void Awake()
        {
            //GameCore.Scene.SetScene(this);
        }

        private void OnDestroy()
        {
            //GameCore.Scene.SetScene(null);
        }

        public SceneContainer GetContainer(string containerName)
        {
            if (containers == null || containers.Length <= 0)
            {
                return null;
            }    

            return Array.Find(containers, c => c.name == containerName);
        }
    }
}

