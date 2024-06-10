using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class SceneInfo
    {
        public int Index { get; private set; }
        public string Name { get; private set; }
        public string Path { get; private set; }    
        public Scene Scene { get; private set; }
        public void SetScene(Scene scene)
        {
            Scene = scene;
            Index = scene.buildIndex;
            Name = scene.name;
            Path = scene.path;
        }
    }
}