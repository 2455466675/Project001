using System;
using System.Collections.Generic;
using System.Reflection;

namespace GameFramework.Core
{
    public class GameMessageDispatcher
    {
        private interface IMessageAction
        {

        }

        private class MessageAction<T> : IMessageAction where T : struct, IGameMessage
        {
            private List<Action<T>> actions;

            public MessageAction()
            {
                actions = new List<Action<T>>();
            }

            public void Invoke(T message)
            {
                for (int i = 0; i < actions.Count; i++)
                {
                    actions[i]?.Invoke(message);
                }
            }

            public void Add(Action<T> action)
            {
                actions.Add(action);
            }

            public void Remove(Action<T> action)
            {
                actions.Remove(action);
            }

            public void Clear()
            {
                actions.Clear();
            }
        }

        private Dictionary<Type, List<IGameMessageHandler>> handlers;
        private Dictionary<Type, IMessageAction> actions;

        internal GameMessageDispatcher()
        {
        }

        public void Init()
        {
            handlers = new Dictionary<Type, List<IGameMessageHandler>>();
            actions = new Dictionary<Type, IMessageAction>();

            var items = Game.GetTypes<GameMessageAttribute>();
            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];

                object obj = Activator.CreateInstance(item.type);
                if (obj is IGameMessageHandler handler)
                {
                    GameMessageAttribute attribute = item.type.GetCustomAttribute(typeof(GameMessageAttribute), false) as GameMessageAttribute;
                    handler.Priority = attribute.priority;

                    Type t = handler.Type;
                    if (!handlers.ContainsKey(t))
                    {
                        handlers.Add(t, new List<IGameMessageHandler>());
                    }
                    handlers[t].Add(handler);
                }
            }

            foreach (var list in handlers.Values)
            {
                list.Sort((a, b) => a.Priority - b.Priority);    
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

            if (actions.ContainsKey(t))
            {
                (actions[t] as MessageAction<T>).Invoke(message);
            }
        }

        public void Subscribe<T>(Action<T> handler) where T : struct, IGameMessage
        {
            Type t = typeof(T);
            if (!actions.ContainsKey(t))
            {
                actions.Add(t, new MessageAction<T>());
            }
            (actions[t] as MessageAction<T>).Add(handler);
        }

        public void Unsubscribe<T>(Action<T> handler) where T : struct, IGameMessage
        {
            Type t = typeof(T);
            if (!actions.ContainsKey(t))
            {
                return;
            }
            (actions[t] as MessageAction<T>).Remove(handler);
        }
    }
}