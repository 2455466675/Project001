using System;

namespace GameFramework
{
    public interface IGameEvent 
    {
        void Init();
        void Register<T>(Action<T> action) where T : IGameEventArgs;
        void Unregister<T>(Action<T> action) where T : IGameEventArgs;
        void Publish<T>(T args) where T : struct, IGameEventArgs;
    }

    public interface IGameEventHandler
    {
        Type Type { get; }
    }

    public interface IGameEventArgs
    {
    }
}
