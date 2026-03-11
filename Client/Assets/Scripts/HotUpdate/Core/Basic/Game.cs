using GameFramework.Core;

namespace GameFramework
{
    public static class Game
    {
        public static GameResourcesManager ResourcesManager { get; private set; }
        public static GameConfigManager ConfigManager { get; private set; }

        private static GameAssemblyManager assemblyManager;
        private static GameSystemManager systemManager;
        private static GameplayManager gameplayManager;

        public static async void Start()
        {
            MDebug.Log("Game Start!");

            ResourcesManager = new GameResourcesManager();
            ConfigManager = new GameConfigManager();
            assemblyManager = new GameAssemblyManager();
            systemManager = new GameSystemManager();
            gameplayManager = new GameplayManager();

            await ResourcesManager.Init();
            await ConfigManager.Init();
            await assemblyManager.Init();
            await systemManager.Init();

            gameplayManager.Init();

            await GameRoot.LoadGameRoot();

            MDebug.Log("Game Start Finish!");

            //GetSystem<GameSaveSystem>().SaveGame(1);
            GetSystem<GameSaveSystem>().LoadGame(1);
        }

        public static void Update(float deltaTime)
        {
            systemManager.Update(deltaTime);
        }

        public static void FixedUpdate(float fixedDeltaTime)
        {
            systemManager.FixedUpdate(fixedDeltaTime);
        }

        public static T GetSystem<T>() where T : class, IGameSystem
        {
            return systemManager.GetSystem<T>();
        }

        public static GameTypeItem[] GetTypes<T>() where T : GameAttribute
        {
            return assemblyManager.GetTypesByGameAttribute<T>();
        }
    }
}