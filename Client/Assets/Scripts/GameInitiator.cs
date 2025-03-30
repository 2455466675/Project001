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
            await GameWorld.Init(config);

            GameWorld.Root.GetComponent<EventComponent>().Publish(new GameStartEventArg());
        }
    }
}