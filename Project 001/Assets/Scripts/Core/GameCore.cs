using System.Collections;
using UnityEngine;
using Game.Cfg;
using Game.UI;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class GameCore
    {
        public static GameInitCfg GameInitCfg { get; private set; }
        public static bool IsEditor { get; private set; }
        public static GameUI UI;
        public static GameCfg GameCfgData;
        public static GameLanguage Language;
        public static GameCoroutine Co;
        public static GameResourceManager ResourceManager;
        public static GameStateController StateController;
        public static GameInputManager InputManager;

        public static GameSystem system;
        private GameCore() { }

        static GameCore() 
        {
            IsEditor = Application.isEditor;
        }

        public static IEnumerator Create(GameInitCfg cfg)
        {
            GameInitCfg = cfg;
            yield return GameCoreImpl.Create();
        }
    }
}