using System.Collections.Generic;
using Config;
using GameFramework.Utility.GameDefine;

namespace GameFramework.Logic
{
    public class StrikeRequest
    {
        public int skillId;
        public int strikeId;
        public int caster;
        public List<int> targets;

        public AbilityStrikeCfg Cfg => Game.Config.Find<AbilityStrikeCfg>(strikeId);

        public StrikeRequest(int skillId, int strikeId, int caster, List<int> targets)
        {
            this.skillId = skillId;
            this.strikeId = strikeId;
            this.caster = caster;
            this.targets = targets;
        }
    }

    public class EffectRequest
    {
        public int skillId;
        public int effectId;
        public int caster;
        public int target;
        public bool isHit;
        public bool isCritical;
        public AbilityEffectCfg Cfg => Game.Config.Find<AbilityEffectCfg>(effectId);
        public EffectRequest()
        {

        }
    }

    public class StrikeResolver
    {
        public void Resolve(IBattleContext context, StrikeRequest req)
        {
            var targets = TargetResolve(context, req);
            for (int i = 0; i < targets.Count; i++)
            {
                var target = targets[i];
                var isHit = HitCheck(context, req, target);
                var isCritical = CriticalCheck(context, req, target);

                for (int j = 0; j < req.Cfg.Effects.Length; j++)
                {
                    EffectRequest eReq = new EffectRequest();
                    eReq.skillId = req.skillId;
                    eReq.effectId = req.Cfg.Effects[j];
                    eReq.caster = req.caster;
                    eReq.target = target;
                    eReq.isHit = isHit;
                    eReq.isCritical = isCritical;
                }
            }            
        }

        private List<int> TargetResolve(IBattleContext context, StrikeRequest req)
        {
            var strikeRange = (StrikeRange)req.Cfg.RangeRule;
            switch (strikeRange)
            {
                case StrikeRange.None:
                    return new List<int>();                    
                case StrikeRange.AllTargets:
                    return req.targets;                    
                case StrikeRange.RandomInTargets:
                    return new List<int>() { req.targets[0] };
                case StrikeRange.AllEnemys:
                    return req.targets;                    
                case StrikeRange.Caster:
                    return new List<int>() { req.caster };
                default:
                    return new List<int>();
            }
        }

        private bool HitCheck(IBattleContext context, StrikeRequest req, int target)
        {
            //req.Cfg.HitRule;
            return true;
        }

        private bool CriticalCheck(IBattleContext context, StrikeRequest req, int target)
        {
            //req.Cfg.CriticalRule;
            return true;
        }
    }

    public class EffectResolver
    {
        public void Resolve(IBattleContext context, EffectRequest req)
        {
            //范围：施法者、受击者

            //阶段：前置目标转移判定（护卫类)
            //依赖：施法者、受击者
            //结果：改变受击者

            //阶段：取施法者与受击者对应的战斗属性计算伤害值,进行Clamp(value, 1, 9999/99999)，下限为1，上限为系统上限9999或施法者突破上限后99999
            //依赖：施法者、受击者
            //结果：产出原始伤害值

            //阶段：后置目标转移判定（反弹类）（护卫不会触发反弹）
            //依赖：施法者、受击者
            //结果：改变受击者

            //阶段：伤害修正管线（免疫判定、护盾吸收、减伤判定\濒死保留1血）
            //依赖：受击者
            //结果：产出修正结果（免疫\吸收\生效, 最终伤害值, 死亡标记)

            //阶段：伤害修正管线2（保留目标1血）
            //依赖：施法者
            //结果：修正最终伤害值, 修改死亡标记

            //阶段：应用修正结果
            //依赖：受击者
            //结果：按修正结果扣除对应属性值\buff

            //阶段：死亡判定
            //依赖：受击者
            //结果：受击者是否死亡

            //阶段：施法者造成伤害后附加效果（吸血、回蓝、某些被动效果）（反弹不触发）
            //依赖：施法者
            //结果：产生额外效果

            //阶段：受击者受到伤害后附加效果（血量变化钩子、反击、某些被动效果）（反弹不触发）
            //依赖：施法者、受击者
            //结果：产生额外效果
        }
    }

    public enum Outcome
    {
        //战斗资源变化，当前血量、当前法力、当前护盾值
        ResourceChanged,
        //添加buff
        AddBuff,
        //移除buff
        RemoveBuff,
        //死亡
        Die,            
    }

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
                    var req = new StrikeRequest(skillId, segmentCfg.Strikes[j], caster, targets);
                    // TODO
                }
            }
        }
    }
}
