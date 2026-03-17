using System;

namespace GameFramework.Core
{
    public interface IGameMessageHandler
    {
        Type Type { get; }
    }

    public abstract class GameMessageHandler<T> : IGameMessageHandler where T : struct, IGameMessage
    {
        public Type Type => typeof(T);

        public abstract void Receive(T message);
    }
}

