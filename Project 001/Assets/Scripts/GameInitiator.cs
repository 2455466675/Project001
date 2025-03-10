using Game.Cfg;
using System.Collections;
using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class GameInitiator : MonoBehaviour
    {
        public GameInitCfg cfg;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            GameWorld.Init();
        }

        private IEnumerator Start()
        {
            yield return GameWorld.Start(cfg);
        }

        private void FixedUpdate()
        {
            GameWorld.Step(Time.fixedDeltaTime);
        }

        //public IEnumerator Start()
        //{
        //    Application.targetFrameRate = 30;
        //    yield return GameCore.Create(cfg);
        //    yield return null;
        //    GameCore.StateController.SwitchState(GameState.LAUNCH);
        //}
    }
}