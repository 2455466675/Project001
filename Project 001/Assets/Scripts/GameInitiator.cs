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

        public IEnumerator Start()
        {
            Application.targetFrameRate = 30;
            yield return GameCore.Create(cfg);
            yield return null;
            GameCore.StateController.SwitchState(GameState.LAUNCH);
        }
    }
}