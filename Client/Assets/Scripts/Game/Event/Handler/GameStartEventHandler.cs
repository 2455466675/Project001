using Game.State;

namespace Game
{
    public struct GameStartEventArg
    {

    }

    /// <summary>
    /// 
    /// </summary>
    [Event]
    public class GameStartEventHandler : GameEvent<GameStartEventArg>
    {
        public override void Run(GameStartEventArg arg)
        {
            Game.State.Switch(GameStateDefine.Login);           
        }
    }
}
