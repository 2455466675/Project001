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
            Application.targetFrameRate = 30;
        }

        private async void Start()
        {

            await Game.Init(config);

            Game.Event.Publish(new GameStartEventArg());
        }
    }
}