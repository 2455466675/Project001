using Cysharp.Threading.Tasks;
using ECS;
using Game.UI;

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
            var rc = Root.AddComponent<ResourceComponent>();
            await rc.Init(config);

            var cc = Root.AddComponent<ConfigComponent>();
            await cc.Init(config);

            var ic = Root.AddComponent<InputComponent>();
            ic.Init();

            var uic = Root.AddComponent<UIComponent>();
            uic.Init();
        }

        public static void Tick(float dt) 
        {
            instance.Update(dt);
        }
    }
}
