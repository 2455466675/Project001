using EC;
using Game.Core;
using Game.System;
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

            InputComponent ic = Instance.AddComponent<InputComponent>();
            yield return ic.Init(intCfg);

            GSMComponent gsmc = Instance.AddComponent<GSMComponent>();
            yield return gsmc.Init(intCfg);

            Instance.AddComponent<SceneComponent>();
            Instance.AddComponent<ActorFactoryComponent>();

            SystemComponent sc = Instance.AddComponent<SystemComponent>();
            yield return sc.Init(intCfg);

            yield return null;

            gsmc.SwitchState(GameState.LAUNCH);
        }

        public static void Step(float dt) 
        {
            Instance.Update(dt);
        }
    }
}
