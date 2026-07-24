using System.Collections.Generic;
using Config;

namespace GameFramework.Logic
{
    public class BattleAction
    {
        public void Execute(IBattleContext context)
        {
            int skillId = 100000;

            var skillEffects = ResolveEffect(skillId);

            int caster = 1;
            List<int> targets = new List<int>() { 11, 12, 13 };
            List<BattleEffect> effects = new List<BattleEffect>();
            for (int i = 0; i < skillEffects.Count; i++)
            {
                var genetators = skillEffects[i];
                for (int j = 0; j < targets.Count; j++)
                {
                        

                    for (int k = 0; k < genetators.Count; k++)
                    {
                        var targetId = targets[j];
                        var effect = genetators[k].Create(caster, targetId);
                        effects.Add(effect);
                    }
                }
            }

            context.Resolver.Append(effects);
        }

        private List<List<BattleEffectGenetator>> ResolveEffect(int skillId)
        {
            var skillCfg = Game.Config.Find<SkillCfg>(skillId);
            var abilityCfg = Game.Config.Find<AbilityCfg>(skillCfg.AbilityId);

            List<List<BattleEffectGenetator>> result = new List<List<BattleEffectGenetator>>(abilityCfg.Segments.Length);
            for (int i = 0; i < abilityCfg.Segments.Length; i++)
            {
                var segmentId = abilityCfg.Segments[i];
                var segmentCfg = Game.Config.Find<AbilitySegmentCfg>(segmentId);

                List<BattleEffectGenetator> effects = new List<BattleEffectGenetator>(segmentCfg.Strikes.Length);
                for (int j = 0; j < segmentCfg.Strikes.Length; j++)
                {
                    var strikeId = segmentCfg.Strikes[j];
                    var strikeCfg = Game.Config.Find<AbilitySegmentStrikeCfg>(strikeId);

                    var effectCfg = Game.Config.Find<EffectCfg>(strikeCfg.Effect);

                    BattleEffectGenetator genetator = new BattleEffectGenetator(effectCfg);
                    effects.Add(genetator);
                }
                result.Add(effects);
            }

            return result;
        }
    }
}
