using GameFramework.Core;

namespace GameFramework
{
    public static class Game
    {
        public static GameAssetsManager Assets { get; private set; }
        public static GameConfigManager Config { get; private set; }
        public static GameMessageDispatcher Message { get; private set; }

        private static GameAssemblyManager assemblyManager;
        private static GameSystemManager systemManager;
        private static GameplayManager gameplayManager;

        // 入口参数用 string[] 承载，便于后续在不改签名的前提下扩展启动参数；约定 args[0] 为资源包名。
        public static async void Start(string[] args)
        {
            MDebug.Log("Game Start!");

            string packageName = (args != null && args.Length > 0) ? args[0] : "DefaultPackage";

            Assets = new GameAssetsManager(new YooAssetProvider(packageName));
            Config = new GameConfigManager();
            Message = new GameMessageDispatcher();
            assemblyManager = new GameAssemblyManager();
            systemManager = new GameSystemManager();
            gameplayManager = new GameplayManager();

            await Assets.Init();
            await Config.Init();
            await assemblyManager.Init();
            await systemManager.Init();

            Message.Init();
            gameplayManager.Init();

            await GameRoot.LoadGameRoot();

            MDebug.Log("Game Start Finish!");

            Message.SendMessage(new GameStartMessage());
        }

        internal static void Update(float deltaTime)
        {
            systemManager.Update(deltaTime);
            gameplayManager.Update(deltaTime);
        }

        internal static void FixedUpdate(float fixedDeltaTime)
        {
            systemManager.FixedUpdate(fixedDeltaTime);
            gameplayManager.FixedUpdate(fixedDeltaTime);
        }

        internal static void LateUpdate(float deltaTime)
        {
            systemManager.LateUpdate(deltaTime);
            gameplayManager.LateUpdate(deltaTime);
        }


        public static T GetSystem<T>() where T : class, IGameSystem
        {
            return systemManager.GetSystem<T>();
        }

        public static T GetModule<T>() where T : class, IGameplay
        {
            return gameplayManager.GetModule<T>();
        }

        public static void GameplayExit()
        {
            gameplayManager.Exit();
        }

        public static GameTypeItem[] GetTypes<T>() where T : GameAttribute
        {
            return assemblyManager.GetTypesByGameAttribute<T>();
        }
    }
}