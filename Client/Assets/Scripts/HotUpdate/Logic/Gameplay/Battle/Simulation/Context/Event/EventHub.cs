using System.Collections.Generic;

namespace GameFramework.Logic
{
    public interface IEventListener
    {
        void OnTrigger(BattleEventType eventType, IBattleContext context, BattleEventArgs args);
    }

    public interface IEventHub
    {
        void Register(BattleEventType eventType, IEventListener listener);
        void Unregister(BattleEventType eventType, IEventListener listener);
        void Fire(BattleEventType eventType, IBattleContext context, BattleEventArgs args);
    }


    public class EventHub : IEventHub
    {
        private readonly Dictionary<BattleEventType, List<IEventListener>> map;

        public EventHub()
        {
            map = new Dictionary<BattleEventType, List<IEventListener>>();
        }

        public void Fire(BattleEventType eventType, IBattleContext context, BattleEventArgs args)
        {
            if (!map.TryGetValue(eventType, out var list))
            {
                return;
            }

            var snapshot = list.ToArray();
            for (int i = 0; i < snapshot.Length; i++)
            {
                snapshot[i].OnTrigger(eventType, context, args);
            }
        }

        public void Register(BattleEventType eventType, IEventListener listener)
        {
            if (!map.TryGetValue(eventType, out var list))
            {
                list = new List<IEventListener>();
                map[eventType] = list;
            }
            list.Add(listener);
        }

        public void Unregister(BattleEventType eventType, IEventListener listener)
        {
            if (map.TryGetValue(eventType, out var list))
            {
                list.Remove(listener);
            }
        }
    }
}
