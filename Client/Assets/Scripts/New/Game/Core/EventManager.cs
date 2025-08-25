using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace GameFramework.Core 
{
    public class GameEventAttribute : GameAttribute 
    {   
    }

    public abstract class GameEventBase<T> : IGameEvent where T : struct, IGameEventArgs
    {
        public Type Type => typeof(T);

        public abstract void Invoke(T arg);
    }

    public class EventManager : IGameModule
    {
        public GameModulePriority Priority => GameModulePriority.EventManager;

        private Dictionary<Type, List<IGameEvent>> m_Events;
        private Dictionary<Type, List<object>> m_Actions;

        public async UniTask Init()
        {
            m_Events = new Dictionary<Type, List<IGameEvent>>();
            m_Actions = new Dictionary<Type, List<object>>();

            AssemblyManager assemblyManager = Game.GetModule<AssemblyManager>();

            Type[] types = assemblyManager.GetTypes<GameEventAttribute>();
            foreach (Type type in types)
            {
                object o = Activator.CreateInstance(type);
                if (o is IGameEvent e)
                {
                    Type t = e.Type;
                    if (!m_Events.ContainsKey(t))
                    {
                        m_Events.Add(t, new List<IGameEvent>());
                    }
                    m_Events[t].Add(e);
                }
            }

            await UniTask.Yield();
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
                List<IGameEvent> events = m_Events[t];
                foreach (var e in events)
                {
                    if (e is GameEventBase<T> ge)
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
    }
}