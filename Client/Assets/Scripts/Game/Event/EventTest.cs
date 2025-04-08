namespace Game
{
    public struct EventTestArg 
    {
        public int x;
        public int y;
        public string name;
    }

    /// <summary>
    /// 
    /// </summary>
    [Event]
    public class EventTest : EventBase<EventTestArg>
    {
        public override void Invoke(EventTestArg arg)
        {
            MLog.Log("EventTest", arg.x, arg.y, arg.name);
        }
    }
}
