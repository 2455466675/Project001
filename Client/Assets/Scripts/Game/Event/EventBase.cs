using Game.Event;
using System;

namespace Game
{
    public interface IEvent 
    {
        public Type Type { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class EventBase<T> : IEvent where T : IEventArgs
    {
        public Type Type => typeof(T);

        public abstract void Invoke(T arg);
    }
}
