using Cysharp.Threading.Tasks;
using Game.Code;
using Game.Config;
using Game.Event;
using Game.Resource;
using Game.State;
using Game.System;
using Game.UI;
using UnityEngine;

namespace Game
{
    public static class Game
    {
        public static GameObject GameRoot { get; private set; }
        public static GameResource Resource { get; private set; }
        public static GameConfig Config { get; private set; }
        public static GameCode Code { get; private set; }
        public static GameEvent Event { get; private set; }
        public static GameScene Scene { get; private set; }
        public static GameState State { get; private set; }
        public static GameUI UI { get; private set; }
        public static GameSystem System { get; private set; }

        public static async UniTask Init(GameInitConfig config) 
        {
            GameRoot = new GameObject("GameRoot");

            Resource = new GameResource();
            await Resource.Init(config);

            Config = new GameConfig();
            await Config.Init(config);

            Code = new GameCode();
            Code.Init();

            Event = new GameEvent();
            Event.Init();

            Scene = new GameScene();
            Scene.Init(config);

            State = new GameState();
            State.Init();

            UI = new GameUI();
            UI.Init(config);

            await UniTask.Yield();
        }
    }
}