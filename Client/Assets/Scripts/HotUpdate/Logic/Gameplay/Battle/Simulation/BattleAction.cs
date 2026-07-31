using System.Collections.Generic;
using Config;

namespace GameFramework.Logic
{
    public class BattleAction
    {
        public void Execute(IBattleContext context)
        {
            int skillId = 100000;


            int caster = 1;
            List<int> targets = new List<int>() { 11, 12, 13 };

            ResolveEffect(skillId);

            List<BattleEffect> effects = new List<BattleEffect>();
            
            context.Resolver.Append(effects);
        }

        private void ResolveEffect(int skillId)
        {
            var skillCfg = Game.Config.Find<SkillCfg>(skillId);
            var abilityCfg = Game.Config.Find<AbilityCfg>(skillCfg.AbilityId);
            var segmentCfg = Game.Config.Find<AbilitySegmentCfg>(abilityCfg.Segments[0]);
            var effectCfg = Game.Config.Find<AbilityEffectCfg>(segmentCfg.HitEffects[0]);
            /*
             * SkillCfg 技能的配置。AbilityId：所具有的能力; TargetRule：决定目标选择，产生targes；HitFormula：命中计算公式；HitModification：命中修正系数
             * AbilityCfg 能力行为配置。Segments: 能力段数； SegmentType ：能力段生效方式，1 = Segments里都是AbilitySegmentCfg，依次生效。2 = 只有一段，随机生效n次， Segments[0]是AbilitySegmentCfg，随机范围[Segments[1],Segments[2]]
             * AbilitySegmentCfg 能力段配置。Range:生效范围，1 = 对targes全部生效；2 = 对targes随机一个生效；FixedEffects：固定效果，无论是否命中都对Range生效；HitEffects：命中效果，命中时对Range生效；CasterEffects：对释放者生效（无论是否命中）
             * AbilityEffectCfg 效果。ElementType：属性；FormulaType：效果公式Id(伤害、回血、buff...)；FormulaValue、BaseValue、Arg1...效果公式参数。
             */
        }
    }
}
