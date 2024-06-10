using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public enum GameState
    {
        None    = 0,
        /// <summary>
        /// Æô¶¯
        /// </summary>
        LAUNCH  = 1,
        /// <summary>
        /// µÇÂ¼
        /// </summary>
        LOGIN   = 2,
        /// <summary>
        /// ÔËÐÐ
        /// </summary>
        PLAYING = 3,
    }

    public enum GameModel 
    { 
        SCENE = 1,
        UI    = 2,
    }

    /// <summary>
    /// 
    /// </summary>
    public class GameStateController : MonoBehaviour, ICore
    {

        public GameState GameState => currStateInst != null ? currStateInst.State : GameState.None;
        public GameModel GameModel {  get; private set; }

        private IGameState currStateInst;

        private Dictionary<GameState, IGameState> states;

        public IEnumerator Init()
        {
            states = new Dictionary<GameState, IGameState>
            {
                {GameState.LAUNCH, new GameLaunchState()},
                {GameState.LOGIN, new GameLoginState()},
                {GameState.PLAYING, new GamePlayingState()},
            };
            yield return null;
        }

        public void Update()
        {
            if (currStateInst == null)
            {
                return;
            }
            currStateInst.OnStay();
        }

        public void SwitchState(GameState state)
        {
            if (!states.ContainsKey(state))
            {
                return;
            }
            if (state == GameState)
            {
                return;
            }
            if (currStateInst != null)
            {
                currStateInst.OnExit(state);
            }
            IGameState gameState = states[state];
            gameState.OnEnter(GameState);
            currStateInst = gameState;
        }

        public void SwitchModel(GameModel model)
        {
            this.GameModel = model;
        }
    }
}