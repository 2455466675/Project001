using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.System
{
    public class ActorContainer : MonoBehaviour
    {
        [SerializeField]
        private Transform actorPool;
        [SerializeField]
        private Transform sceneUnit;

        public Transform ActorPool => actorPool;
        public Transform SceneUnit => sceneUnit;

        private void Awake()
        {
            actorPool.gameObject.SetActive(false);
        }
    }
}