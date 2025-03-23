using System.Collections.Generic;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class StateComponent : ECS.Entity
    {
        private List<GameState> states;

        private GameState currentState;

        public void Init() 
        {
            states = new List<GameState>
            {
                new GameLoginState(),
                new GamePlayingState(),
            };
        }

        public void Switch(GameStateDefine define) 
        {
            GameState state = states.Find(s => s.Define == define);
            if (state == null) 
            {
                return;
            }

            currentState?.Exit();
            currentState = state;
            currentState?.Enter();
        }
    }
}
