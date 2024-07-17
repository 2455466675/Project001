using System;
using System.Collections;
using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
	public class GameScene : MonoBehaviour, ICore
    {

        public SceneMap CurrScene {get; private set;}

        public IEnumerator Init()
        {
            yield return null;
        }

        public void SetScene(SceneMap scene)
        {
            CurrScene = scene;
        }

        public SceneContainer GetContainer(string containerName)
        {
            if (CurrScene == null)
            {
                return null;
            }

            return CurrScene.GetContainer(containerName);
        }
    }
}

