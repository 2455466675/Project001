using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameFramework.Core
{
    [Serializable]
    internal class SceneConfig
    {
        [SerializeField]
        private int id;
        [SerializeField]
        private string path;
        [SerializeField]
        private LoadSceneMode loadSceneMode;
        [SerializeField]
        private string desc;

        public int Id => id;
        public string Path => path;
        public LoadSceneMode LoadSceneMode => loadSceneMode;
    }
}