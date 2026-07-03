using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace GameFramework.Logic
{
    public interface IActionSchedule
    {
        void Rebuild(IBattleContext context);

        bool Dequeue(out int battleID);
    }

    public interface IEffectResolver
    {
        void Append(List<BattleEffect> effects);
        void Push(BattleEffect effect);
        void ResolveAll(IBattleContext context);
    }

    public interface IViewProjector
    {
        void NextBatch();
        void Record(BattleViewCommand command);
        UniTask Flush();
    }

    public interface IEventHub
    {
        void Register(BattleEventType eventType, IEventListener listener);
        void Unregister(BattleEventType eventType, IEventListener listener);
        void Fire(BattleEventType eventType, IBattleContext context, BattleEventArgs args);    
    }


    public interface IBattleContext
    {
        IActionSchedule Schedule { get; }
        IEffectResolver Resolver { get; }
        IViewProjector Projector { get; }
        IEventHub EventHub { get; }

        bool CheckFinish();
    }

    public class BattleContext : IBattleContext
    {
        public IActionSchedule Schedule { get; private set; }
        public IEffectResolver Resolver { get; private set; }
        public IViewProjector Projector { get; private set; }
        public IEventHub EventHub { get; private set; }

        public BattleContext()
        {
            Schedule = new ActionSchedule();
            Resolver = new EffectResolver();
            Projector = new ViewProjector();
            EventHub = new EventHub();
        }

        public bool CheckFinish()
        {
            return false;
        }
    }

    public class ActionSchedule : IActionSchedule
    {
        private readonly List<int> schedule;

        public ActionSchedule()
        {
            schedule = new List<int>();
        }

        public bool Dequeue(out int battleID)
        {
            if (schedule.Count == 0)
            {
                battleID = -1;
                return false;
            }
            else
            {
                battleID = schedule[0];
                schedule.RemoveAt(0);
                return true;
            }
        }

        public void Rebuild(IBattleContext context)
        {
            schedule.Clear();
            schedule.Add(1);
            schedule.Add(2);
            schedule.Add(3);
            schedule.Add(4);
        }
    }

    public class EffectResolver : IEffectResolver
    {
        private readonly Queue<Stack<BattleEffect>> pending;

        public EffectResolver()
        {
            pending = new Queue<Stack<BattleEffect>>();
        }

        public void Append(List<BattleEffect> effects)
        {
            if (effects == null || effects.Count == 0)
            {
                return;
            }
            effects.Reverse(); //为了保持元素顺序，反转列表，new Stack()时会将元素依次入栈，相当于再次反转了元素顺序
            Stack<BattleEffect> stack = new Stack<BattleEffect>(effects);
            pending.Enqueue(stack);
        }

        public void Push(BattleEffect effect)
        {
            if(pending.TryPeek(out var stack))
            {
                stack?.Push(effect);
            }
        }

        public void ResolveAll(IBattleContext context)
        {
            while (pending.Count > 0)
            {
                if (pending.TryPeek(out var stack))
                {
                    context.Projector.NextBatch();
                    while (stack.Count > 0)
                    {
                        var effect = stack.Pop();
                        effect.Apply(context);
                    }
                }
                pending.Dequeue();
            }
        }
    }

    public class ViewProjector : IViewProjector
    {
        private readonly List<BattleViewCommand> commands;

        private int currentBatch;

        public ViewProjector()
        {
            commands = new List<BattleViewCommand>();
            currentBatch = 0;
        }

        public void NextBatch()
        {
            currentBatch++;
        }

        public void Record(BattleViewCommand command)
        {
            command.Batch = currentBatch;
            commands.Add(command);
        }

        public async UniTask Flush()
        {
            while (commands.Count > 0)
            {
                int batch = commands[0].Batch;
                var list = commands.FindAll(c => c.Batch == batch);

                var group = new List<UniTask>(list.Count);
                for (int i = 0; i < list.Count; i++)
                {
                    group.Add(list[i].Play());
                }

                await UniTask.WhenAll(group);

                commands.RemoveAll(c => c.Batch == batch);
            }

            //commands.Clear();
        }
    }

    public enum BattleEventType
    {

    }

    public class BattleEventArgs
    {

    }

    public interface IEventListener
    {
        void OnTrigger(BattleEventType eventType, IBattleContext context, BattleEventArgs args);
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
