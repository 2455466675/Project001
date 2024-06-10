using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGameState
    {
        GameState State {get;}

        void OnEnter(GameState preGameState);
        void OnStay();
        void OnExit(GameState nextGameState);
    }
}