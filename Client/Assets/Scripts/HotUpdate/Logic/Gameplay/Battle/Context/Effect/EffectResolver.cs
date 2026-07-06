using System.Collections.Generic;

namespace GameFramework.Logic
{
    public interface IEffectResolver
    {
        void Append(List<BattleEffect> effects);
        void Push(BattleEffect effect);
        void ResolveAll(IBattleContext context);
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
            if (pending.TryPeek(out var stack))
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
}
