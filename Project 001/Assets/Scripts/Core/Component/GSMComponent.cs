using System.Collections;
using System.Collections.Generic;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class GSMComponent : EC.Component, IInitializable
    {
        public GameState CurrGameState { get; private set; }

        private List<IGameState> states;

        public IEnumerator Init(GameInitCfg intCfg)
        {
            states = new List<IGameState>
            {
                new GameLaunchState(),
                new GameLoginState(),
                new GamePlayingState(),
            };

            yield return null;
        }

        public void SwitchState(GameState state)
        {
            if (CurrGameState == state)
            {
                return;
            }

            IGameState currState = states.Find(x => x.State == CurrGameState);
            IGameState nextState = states.Find(x => x.State == state);

            currState?.OnExit(state);
            nextState?.OnEnter(CurrGameState);
            CurrGameState = state;
        }
    }
}
