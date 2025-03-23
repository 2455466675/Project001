using ECS;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class SceneEntity : Entity
    {
        public int SceneId => data.Id;
        public int BuildIndex => data.BuildIndex;
        public string Name => data.Name;
        public string Path => data.Path;
        public LoadSceneMode LoadSceneMode => data.LoadSceneMode;
        public Scene Scene => data.LoadedScene;

        private SceneData data;

        public void Init(SceneData data)
        {
            this.data = data;
        }

        public void SetActive(bool active)
        {
            GameObject[] objects = Scene.GetRootGameObjects();
            foreach (GameObject obj in objects)
            {
                obj.SetActive(active);
            }
        }
    }
}
