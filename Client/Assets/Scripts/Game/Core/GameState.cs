using System.Collections.Generic;

namespace Game.State
{
    public class GameState
    {
        private List<StateBase> states;
        private StateBase currentState;

        public void Init()
        {
            states = new List<StateBase>
            {
                new GameLoginState(),
                new GamePlayingState(),
            };
        }

        public void Switch(GameStateDefine define)
        {
            StateBase state = states.Find(s => s.Define == define);
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