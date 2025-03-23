using Cysharp.Threading.Tasks;
using ECS;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class GameWorld : World
    {
        private static GameWorld instance;
        public static Entity Root => instance.root;

        public static void Initialize() 
        {
            if (instance != null) 
            {
                return;
            }
            instance = new GameWorld();
        }

        public static async UniTask Start(GameInitConfig config) 
        {
            var resourceComponent = Root.AddComponent<ResourceComponent>();
            await resourceComponent.Init(config);

            var configComponent = Root.AddComponent<ConfigComponent>();
            await configComponent.Init(config);

            var codeComponent = Root.AddComponent<CodeComponent>();
            codeComponent.Init();

            var eventComponent = Root.AddComponent<EventComponent>();
            eventComponent.Init();

            var inputComponent = Root.AddComponent<InputComponent>();
            inputComponent.Init();

            var uiComponent = Root.AddComponent<UIComponent>();
            uiComponent.Init(config);

            var sceneComponent = Root.AddComponent<SceneComponent>();
            sceneComponent.Init(config);

            var stateComponent = Root.AddComponent<StateComponent>();
            stateComponent.Init();

            //LoginState
            //PlayingState
        }

        public static void Tick(float dt) 
        {
            instance.Update(dt);
        }
    }
}
