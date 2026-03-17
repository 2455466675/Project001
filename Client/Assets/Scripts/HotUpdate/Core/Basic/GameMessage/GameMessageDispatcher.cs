using System;
using System.Collections.Generic;

namespace GameFramework.Core
{
    public class GameMessageDispatcher
    {
        private Dictionary<Type, List<IGameMessageHandler>> handlers;
        private Dictionary<Type, List<object>> subscribers;

        internal GameMessageDispatcher()
        {
        }

        public void Init()
        {
            handlers = new Dictionary<Type, List<IGameMessageHandler>>();
            subscribers = new Dictionary<Type, List<object>>();

            var items = Game.GetTypes<GameMessageAttribute>();
            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                object obj = Activator.CreateInstance(item.type);
                if (obj is IGameMessageHandler handler)
                {
                    Type t = handler.Type;
                    if (!handlers.ContainsKey(t))
                    {
                        handlers.Add(t, new List<IGameMessageHandler>());
                    }
                    handlers[t].Add(handler);
                }
            }
        }

        public void SendMessage<T>(T message) where T : struct, IGameMessage
        {
            Type t = typeof(T);
            if (handlers.ContainsKey(t))
            {
                List<IGameMessageHandler> list = handlers[t];
                foreach (var item in list)
                {
                    if (item is GameMessageHandler<T> handler)
                    {
                        handler.Receive(message);
                    }
                }
            }

            if (subscribers.ContainsKey(t))
            {
                List<object> list = subscribers[t];
                foreach (var item in list)
                {
                    if (item is Action<T> handler)
                    {
                        handler.Invoke(message);
                    }
                }
            }
        }

        public void Subscribe<T>(Action<T> handler) where T : struct, IGameMessage
        {
            Type t = typeof(T);
            if (!subscribers.ContainsKey(t))
            {
                subscribers.Add(t, new List<object>());
            }
            subscribers[t].Add(handler);
        }

        public void Unsubscribe<T>(Action<T> handler) where T : struct, IGameMessage
        {
            Type t = typeof(T);
            if (!subscribers.ContainsKey(t))
            {
                return;
            }
            subscribers[t].Remove(handler);
        }
    }
}