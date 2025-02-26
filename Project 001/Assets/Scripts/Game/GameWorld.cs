using EC;
using Game.Core;
using Game.UI;
using System.Collections;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class GameWorld : World
    {
        public static GameWorld Instance { get; private set; }

        public static void Init() 
        {
            Instance = new GameWorld();
        }

        public static IEnumerator Start(GameInitCfg intCfg) 
        {
            ResourceComponent rc = Instance.AddComponent<ResourceComponent>();
            yield return rc.Init(intCfg);

            ConfigComponent cc = Instance.AddComponent<ConfigComponent>();
            yield return cc.Init(intCfg);

            UIComponent uic = Instance.AddComponent<UIComponent>();
            yield return uic.Init(intCfg);

            uic.ShowPanel<TestPanelController>(1);

            yield return null;
        }

        public static void Step(float dt) 
        {
            Instance.Update(dt);
        }
    }
}
