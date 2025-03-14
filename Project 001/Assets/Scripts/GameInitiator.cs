using UnityEngine;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class GameInitiator : MonoBehaviour
    {        
        public GameInitConfig config;

        private void Awake()
        {
            GameWorld.Initialize();   
        }

        private async void Start()
        {
            await GameWorld.Start(config);
        }

        private void Update() 
        {
            GameWorld.Tick(Time.deltaTime);
        }
    }
}