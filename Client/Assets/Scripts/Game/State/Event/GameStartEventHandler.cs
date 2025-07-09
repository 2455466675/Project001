using Game.Event;
using Game.State;

namespace Game
{
    public struct GameStartEventArgs : IEventArgs
    {

    }

    /// <summary>
    /// 
    /// </summary>
    [Event]
    public class GameStartEventHandler : EventBase<GameStartEventArgs>
    {
        public override void Invoke(GameStartEventArgs args)
        {
            Game.State.Switch(GameStateDefine.Login);           
        }
    }
}
