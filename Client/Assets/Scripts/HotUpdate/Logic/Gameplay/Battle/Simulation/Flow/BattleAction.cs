using System.Collections.Generic;
using Config;
using GameFramework.Utility.GameDefine;

namespace GameFramework.Logic
{
    public class BattleAction
    {
        public int skillId;
        public int caster;
        public List<int> targets;

        public void Execute(IBattleContext context)
        {
            skillId = 100000;
            caster = 1;
            targets = new List<int>() { 11, 12, 13 };

            ResolveEffect(context);
        }

        private void ResolveEffect(IBattleContext context)
        {
            var skillCfg = Game.Config.Find<SkillCfg>(skillId);
            var abilityCfg = Game.Config.Find<AbilityCfg>(skillCfg.AbilityId);
            for (int i = 0; i < abilityCfg.Segments.Length; i++)
            {
                var segmentCfg = Game.Config.Find<AbilitySegmentCfg>(abilityCfg.Segments[i]);
                for (int j = 0; j < segmentCfg.Strikes.Length; j++)
                {
                    var strikeCfg = Game.Config.Find<AbilityStrikeCfg>(segmentCfg.Strikes[j]);

                    var strikeRange = (StrikeRange)strikeCfg.RangeRule;
                    if (strikeRange == StrikeRange.AllTargets)
                    {
                        for (int k = 0; k < targets.Count; k++)
                        {
                            var targetId = targets[k];

                            // strikeCfg.HitRule 命中计算
                            // strikeCfg.CriticalRule 暴击计算

                            for (int m = 0; m < strikeCfg.Effects.Length; m++)
                            {
                                var effectCfg = Game.Config.Find<AbilityEffectCfg>(strikeCfg.Effects[m]);
                                //对目标应用效果
                                var formulaType = (FormulaType)effectCfg.Formula;
                                if (formulaType == FormulaType.Damage)
                                {
                                    var unit = context.GetBattleUnit(targetId);

                                    //计算伤害
                                    int value = effectCfg.BaseValue;

                                    var mod = new PropertyModifier()
                                    {
                                        Target = PropertyDefine.CurHp,
                                        Op = ModifierOp.Add,
                                        Value = -value,
                                    };

                                    unit.GetComponent<PropertyComponent>().AddModifier(mod);
                                }
                                if (formulaType == FormulaType.AddBuff)
                                {
                                    //添加buff
                                }
                            }
                        }
                    }

                    if (strikeRange == StrikeRange.Caster)
                    {
                        //strikeCfg.HitRule 通常必中
                        //strikeCfg.CriticalRule 通常不暴击
                        for (int m = 0; m < strikeCfg.Effects.Length; m++)
                        {
                            var effectCfg = Game.Config.Find<AbilityEffectCfg>(strikeCfg.Effects[m]);
                            //对caster应用效果
                        }
                    }
                }
            }
        }
    }
}
