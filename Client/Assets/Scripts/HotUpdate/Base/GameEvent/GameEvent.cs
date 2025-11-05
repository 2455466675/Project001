using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace GameFramework 
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GameEventAttribute : GameAttribute 
    {   
    }

    public abstract class GameEventHandlerBase<T> : IGameEventHandler where T : struct, IGameEventArgs
    {
        public Type Type => typeof(T);

        public abstract void Invoke(T arg);
    }

    public abstract class GameAsyncEventHandlerBase<T> : IGameEventHandler where T : struct, IGameEventArgs
    {
        public Type Type => typeof(T);

        public abstract UniTask Invoke(T arg);
    }

    internal class GameEvent : IGameEvent
    {
        private Dictionary<Type, List<IGameEventHandler>> m_Events;
        private Dictionary<Type, List<object>> m_Actions;

        internal GameEvent() { }

        public void Init()
        {
            m_Events = new Dictionary<Type, List<IGameEventHandler>>();
            m_Actions = new Dictionary<Type, List<object>>();

            Type[] types = Game.GetTypes<GameEventAttribute>();
            foreach (Type type in types)
            {
                object o = Activator.CreateInstance(type);
                if (o is IGameEventHandler e)
                {
                    Type t = e.Type;
                    if (!m_Events.ContainsKey(t))
                    {
                        m_Events.Add(t, new List<IGameEventHandler>());
                    }
                    m_Events[t].Add(e);
                }
            }
        }

        public void Register<T>(Action<T> action) where T : IGameEventArgs
        {
            Type t = typeof(T);
            if (!m_Actions.ContainsKey(t))
            {
                m_Actions.Add(t, new List<object>());
            }
            m_Actions[t].Add(action);
        }

        public void Unregister<T>(Action<T> action) where T : IGameEventArgs
        {
            Type t = typeof(T);
            if (!m_Actions.ContainsKey(t))
            {
                return;
            }
            m_Actions[t].Remove(action);
        }

        public void Publish<T>(T args) where T : struct, IGameEventArgs
        {
            Type t = typeof(T);
            if (m_Events.ContainsKey(t))
            {
                List<IGameEventHandler> events = m_Events[t];
                foreach (var e in events)
                {
                    if (e is GameEventHandlerBase<T> ge)
                    {
                        ge.Invoke(args);
                    }
                }
            }

            if (m_Actions.ContainsKey(t))
            {
                List<object> events = m_Actions[t];
                foreach (var e in events)
                {
                    if (e is Action<T> a)
                    {
                        a.Invoke(args);
                    }
                }
            }
        }

        public async UniTask PublishAsync<T>(T args) where T : struct, IGameEventArgs 
        {
            Type t = typeof(T);
            if (m_Events.ContainsKey(t))
            {
                List<IGameEventHandler> events = m_Events[t];
                foreach (var e in events)
                {
                    if (e is GameAsyncEventHandlerBase<T> ge)
                    {
                        await ge.Invoke(args);
                    }
                }
            }
        }
    }
}