using ECS;
using System;
using System.Collections.Generic;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class EventComponent : Entity
    {
        private Dictionary<Type, List<IEvent>> allEvent;

        private Dictionary<Type, List<object>> actions;

        public void Init()
        {
            allEvent = new Dictionary<Type, List<IEvent>>();
            actions = new Dictionary<Type, List<object>>();

            List<Type> types = GameWorld.Root.GetComponent<CodeComponent>().GetTypes<EventAttribute>();
            foreach (Type type in types) 
            {
                object o = Activator.CreateInstance(type);
                if (o is IEvent e)
                {
                    Type t = e.Type;
                    if (!allEvent.ContainsKey(t)) 
                    {
                        allEvent.Add(t, new List<IEvent>());
                    }
                    allEvent[t].Add(e);
                }
            }
        }

        public void Publish<T>(T arg) where T : struct
        {
            Type t = typeof(T);
            if (allEvent.ContainsKey(t)) 
            {
                List<IEvent> events = allEvent[t];
                foreach (IEvent e in events)
                {
                    if (e is GameEvent<T> ge)
                    {
                        ge.Run(arg);
                    }
                }
            }

            if (actions.ContainsKey(t)) 
            {
                List<object> events = actions[t];
                foreach (var e in events)
                {
                    if (e is Action<T> a) 
                    {
                        a.Invoke(arg);
                    }
                }
            }
        }

        public void Register<T>(Action<T> action) where T : struct
        {
            Type t = typeof(T);
            if (!actions.ContainsKey(t))
            {
                actions.Add(t, new List<object>());
            }
            actions[t].Add(action);
        }

        public void Unregister<T>(Action<T> action) where T : struct 
        {
            Type t = typeof(T);
            if (!actions.ContainsKey(t))
            {
                return;
            }
            actions[t].Remove(action);
        }
    }
}
