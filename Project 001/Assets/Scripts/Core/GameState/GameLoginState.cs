using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class GameLoginState : IGameState
    {
        public GameState State { get; }

        public GameLoginState()
        {
            State = GameState.LOGIN;
        }

        public void OnEnter(GameState preGameState)
        {
            Debug.Log("ÓÎÏ·×´Ì¬£ºµÇÂ¼");
            GameCore.StateController.SwitchModel(GameModel.UI);
            GameCore.UI.OpenWinCommond(100001);
            GameCore.UI.OpenWinCommond(100002);
            //GameCore.UI.OpenWinAsync(100002, 1);
        }

        public void OnExit(GameState nextGameState)
        {

        }

        public void OnStay()
        {

        }
    }
}