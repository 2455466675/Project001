using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class GamePlayingState : IGameState
    {
        public GameState State { get; }

        public GamePlayingState()
        {
            State = GameState.PLAYING;
        }

        public void OnEnter(GameState preGameState)
        {
            Debug.Log("ÓÎÏ·×´Ì¬£ºÔËÐÐ");
        }

        public void OnExit(GameState nextGameState)
        {

        }

        public void OnStay()
        {

        }
    }
}