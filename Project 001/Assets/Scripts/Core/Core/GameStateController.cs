using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class GameStateController : MonoBehaviour, ICore
    {
        /// <summary>
        /// 游戏当前状态
        /// </summary>
        public GameState GameState => currStateInst != null ? currStateInst.State : GameState.None;
        /// <summary>
        /// 游戏当前操作模式
        /// </summary>
        public GameMode GameMode => currModeInst != null ? currModeInst.Mode : GameMode.SCENE;

        private IGameState currStateInst;

        private Dictionary<GameState, IGameState> states;

        private IGameMode currModeInst;

        private Dictionary<GameMode, IGameMode> modes;

        public IEnumerator Init()
        {
            states = new Dictionary<GameState, IGameState>
            {
                {GameState.LAUNCH, new GameLaunchState()},
                {GameState.LOGIN, new GameLoginState()},
                {GameState.PLAYING, new GamePlayingState()},
            };

            modes = new Dictionary<GameMode, IGameMode>
            {
                {GameMode.UI, new GameUIMode()},
                {GameMode.SCENE, new GameSceneMode()},
            };
            yield return null;
        }

        /// <summary>
        /// 切换游戏状态
        /// </summary>
        /// <param name="state"></param>
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
            currStateInst?.OnExit(state);
            IGameState gameState = states[state];
            gameState.OnEnter(GameState);
            currStateInst = gameState;
        }

        /// <summary>
        /// 切换游戏操作模式
        /// </summary>
        /// <param name="model"></param>
        public void SwitchModel(GameMode model, ModeArg arg = null)
        {
            if (!modes.ContainsKey(model))
            {
                return;
            }
            if (model == GameMode)
            {
                return;
            }
            currModeInst?.OnExit();
            IGameMode gameModel = modes[model];
            gameModel.OnEnter(arg);
            currModeInst = gameModel;
        }
    }
}