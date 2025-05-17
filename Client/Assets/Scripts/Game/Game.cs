using Cysharp.Threading.Tasks;
using UnityEngine;
using Game.Code;
using Game.Config;
using Game.Event;
using Game.Resource;
using Game.State;
using Game.GSystem;
using Game.UI;

namespace Game
{
    public static class Game
    {
        public static GameObject Root { get; private set; }
        public static GameResource Resource { get; private set; }
        public static GameConfig Config { get; private set; }
        public static GameCode Code { get; private set; }
        public static GameEvent Event { get; private set; }
        public static GameScene Scene { get; private set; }
        public static GameState State { get; private set; }
        public static GameUI UI { get; private set; }
        public static GameSystem System { get; private set; }

        public static GameCommand GM { get; private set; }

        private static bool IsInited;

        public static async UniTask Init(GameInitConfig config) 
        {
            IsInited = false;

            Root = new GameObject("GameRoot");
            Root.AddComponent<GameRoot>();

            Resource = new GameResource();
            await Resource.Init(config);

            Config = new GameConfig();
            await Config.Init(config);

            Code = new GameCode();
            Code.Init();

            Event = new GameEvent();
            Event.Init();

            Scene = new GameScene();
            Scene.Init();

            State = new GameState();
            State.Init();

            UI = new GameUI();
            UI.Init();

            System = new GameSystem();
            System.Init();

            GM = new GameCommand();
            GM.Init();

            await UniTask.Yield();

            IsInited = true;
        }

        public static void Update(float dt) 
        {
            if (!IsInited) 
            {
                return;
            }

        }

        public static void FixedUpdate(float fdt) 
        {
            if (!IsInited)
            {
                return;
            }

            UI.FixedUpdate(fdt);
            System.FixedUpdate(fdt);
        }
    }
}