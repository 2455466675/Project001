using Game.Event;

namespace Game
{
    public struct EventTestEventArgs : IEventArgs
    {
        public int x;
        public int y;
        public string name;
    }

    /// <summary>
    /// 
    /// </summary>
    [Event]
    public class EventTest : EventBase<EventTestEventArgs>
    {
        public override void Invoke(EventTestEventArgs arg)
        {
            MLog.Log("EventTest", arg.x, arg.y, arg.name);
        }
    }
}
