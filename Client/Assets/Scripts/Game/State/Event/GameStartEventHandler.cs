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
    public class GameStartEventHandler : EventBase<GameStartEventArg>
    {
        public override void Invoke(GameStartEventArg arg)
        {
            Game.State.Switch(GameStateDefine.Login);           
        }
    }
}
