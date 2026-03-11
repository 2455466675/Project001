using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GameFrameworkAOT
{
    public class GameInitiator : MonoBehaviour
    {
        public GameInitConfig config;

        private void Awake()
        {

        }

        private async void Start()
        {
            HotUpdateManager hotUpdateManager = new HotUpdateManager(config);
            await hotUpdateManager.Start();

            GameEntryManager gameEntryManager = new GameEntryManager(config);
            await gameEntryManager.Start();
        }
    }
}