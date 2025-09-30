using System;

namespace GameFramework 
{
    public interface IGameEvent
    {
        Type Type { get; }
    }

    public interface IGameEventArgs
    {
    }
}