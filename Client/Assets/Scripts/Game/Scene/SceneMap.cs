using Eflatun.SceneReference;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    [Serializable]
    public class SceneData 
    {
        [SerializeField]
        private int id;
        [SerializeField]
        private LoadSceneMode loadSceneMode;
        [SerializeField]
        private SceneReference reference;

        public int Id => id;
        public LoadSceneMode LoadSceneMode => loadSceneMode;
        public int BuildIndex => reference.BuildIndex;
        public string Name => reference.Name;
        public string Path => reference.Path;
        public Scene LoadedScene => reference.LoadedScene;
    }

    [CreateAssetMenu(menuName = "MyMenu/Create SceneMap")]
    public class SceneMap : ScriptableObject
    {
        [SerializeField]
        private List<SceneData> data;

        public SceneData GetSceneData(int id) 
        {
            if (data == null || data.Count == 0) 
            {
                return null;
            }

            return data.Find(s => s.Id == id);
        }
    }
}