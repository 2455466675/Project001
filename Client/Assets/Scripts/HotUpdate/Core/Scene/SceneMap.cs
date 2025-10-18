using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameFramework
{
    [Serializable]
    public class SceneConfig 
    {
        [SerializeField]
        private int id;
        [SerializeField]
        private LoadSceneMode loadSceneMode;
        [SerializeField]
        private string path;
        [SerializeField]
        private string desc;

        public int Id => id;
        public LoadSceneMode LoadSceneMode => loadSceneMode;
        public string Path => path;
    }

    [CreateAssetMenu(menuName = "MyMenu/Create SceneMap")]
    public class SceneMap : ScriptableObject
    {
        [SerializeField]
        private List<SceneConfig> data;

        internal SceneConfig GetSceneConfig(int id) 
        {
            if (data == null || data.Count == 0) 
            {
                return null;
            }

            return data.Find(s => s.Id == id);
        }

        internal List<SceneConfig> GetSceneConfigList()
        {
            if (data == null)
            {
                return new List<SceneConfig>();
            }
            return data;
        }
    }
}