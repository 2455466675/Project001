using Game.Cfg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class GameInitiator : MonoBehaviour
    {
        public GameInitCfg cfg;

        public IEnumerator Start()
        {
            yield return GameCore.Create(cfg);
            yield return null;
            GameCore.StateController.SwitchState(GameState.LAUNCH);
        }
    }
}