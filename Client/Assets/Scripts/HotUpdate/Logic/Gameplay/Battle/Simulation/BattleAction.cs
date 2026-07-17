using System.Collections.Generic;

namespace GameFramework.Logic
{
    public class BattleAction
    {
        public void Execute(IBattleContext context)
        {
            int caster = 1;
            List<int> targets = new List<int>() { 11, 12, 13 };

            foreach (var target in targets)
            {
                List<BattleEffect> effects = new List<BattleEffect>();

                var effect = new NormalDamageEffect(); // Pool
                effect.Reset(caster, target);

                effects.Add(effect);
                context.Resolver.Append(effects);
            }            
        }
    }
}
