using Game.Cfg;
using Game.System;
using Game.UI;
using System.Collections;
using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class GameCoreImpl : MonoBehaviour, ICore
    {
        public static GameCoreImpl Inst {get; private set;}

        public static IEnumerator Create()
        {
            GameObject gameObject = new GameObject("GameCore");
            DontDestroyOnLoad(gameObject);
            Inst = gameObject.AddComponent<GameCoreImpl>();
            yield return Inst.Init();
        }

        public IEnumerator Init()
        {
            GameCore.ResourceManager = gameObject.AddComponent<GameResourceManager>();
            yield return GameCore.ResourceManager.Init();

            GameCore.Cfg = gameObject.AddComponent<GameCfg>();
            yield return GameCore.Cfg.Init();

            GameCore.UI = gameObject.AddComponent<GameUI>();
            yield return GameCore.UI.Init();

            GameCore.Coroutine = gameObject.AddComponent<GameCoroutine>();
            yield return GameCore.Coroutine.Init();

            GameCore.StateController = gameObject.AddComponent<GameStateController>();
            yield return GameCore.StateController.Init();

            GameCore.Input = gameObject.AddComponent<GameInputSystem>();
            yield return GameCore.Input.Init();

            GameCore.Scene = gameObject.AddComponent<GameScene>();
            yield return GameCore.Scene.Init();

            GameCore.System = new GameSystem();
        }
    }
}