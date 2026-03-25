using System;

namespace GameFramework.Core
{
    public interface IGameMessageHandler
    {
        Type Type { get; }
        int Priority { get; set; }
    }

    public abstract class GameMessageHandler<T> : IGameMessageHandler where T : struct, IGameMessage
    {
        public Type Type => typeof(T);
        public int Priority { get; set; }

        public abstract void Receive(T message);
    }
}

