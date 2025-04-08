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
    public abstract class EventBase<T> : IEvent where T : struct
    {
        public Type Type => typeof(T);

        public abstract void Invoke(T arg);
    }
}
