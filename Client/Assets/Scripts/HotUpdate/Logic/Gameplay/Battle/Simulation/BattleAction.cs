using System.Collections.Generic;

namespace GameFramework.Logic
{
    public class BattleAction
    {
        public void Execute(IBattleContext context)
        {
            //产出BattleEffect

            List<BattleEffect> effects = new List<BattleEffect>();
            effects.Add(new NormalDamageEffect());
            context.Resolver.Append(effects);
        }
    }
}
