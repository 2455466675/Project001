using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
    public enum GameState
    {
        None    = 0,
        /// <summary>
        /// 启动
        /// </summary>
        LAUNCH  = 1,
        /// <summary>
        /// 登录
        /// </summary>
        LOGIN   = 2,
        /// <summary>
        /// 运行
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
        /// <summary>
        /// 游戏当前状态
        /// </summary>
        public GameState GameState => currStateInst != null ? currStateInst.State : GameState.None;
        /// <summary>
        /// 游戏当前操作模式
        /// </summary>
        public GameModel GameModel {  get; private set; }
        /// <summary>
        /// 是否是场景模式
        /// </summary>
        public bool IsSceneModel => GameModel == GameModel.SCENE;
        /// <summary>
        /// 是否是UI模式
        /// </summary>
        public bool IsUIModel => GameModel == GameModel.UI;

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
            if (currStateInst != null)
            {
                currStateInst.OnExit(state);
            }
            IGameState gameState = states[state];
            gameState.OnEnter(GameState);
            currStateInst = gameState;
        }

        /// <summary>
        /// 切换游戏操作模式
        /// </summary>
        /// <param name="model"></param>
        public void SwitchModel(GameModel model)
        {
            this.GameModel = model;
        }
    }
}