using System;
using System.Collections.Generic;

namespace GameFramework.Logic
{
    public interface IDamagePipeline
    {

    }

    public interface IDamageStage
    {
        void Process(DamagePipelineContext ctx);
    }

    [Flags]
    public enum DamageFlags
    {
        None     = 0,
        Hit      = 1 << 0,
        Immunity = 1 << 1,
        Critical = 1 << 2,
        Absorbed = 1 << 3,
    }

    public class DamagePipelineContext
    {
        public int BaseValue;
        public int FinalValue;
        public int CostShieldValue;

        public DamageFlags Mask;
        public bool IsHit => Mask.HasFlag(DamageFlags.Hit);
        public bool IsFluent => IsHit && !Mask.HasFlag(DamageFlags.Immunity);
        public bool IsCrit => IsFluent && Mask.HasFlag(DamageFlags.Critical);
        public bool IsAbsorbed => IsFluent && Mask.HasFlag(DamageFlags.Absorbed);
    }

    public class DamagePipeline : IDamagePipeline
    {
        private class HitCheckStage : IDamageStage
        {
            public void Process(DamagePipelineContext ctx)
            {
                bool isHit = true;
                ctx.Mask |= isHit ? DamageFlags.Hit : DamageFlags.None;
            }
        }

        private class ImmunityCheckStage : IDamageStage
        {
            public void Process(DamagePipelineContext ctx)
            {
                if (ctx.IsHit)
                {
                    bool isImmunity = false;
                    ctx.Mask |= isImmunity ? DamageFlags.Immunity : DamageFlags.None;
                }
            }
        }

        private class CritCheckStage : IDamageStage
        {
            public void Process(DamagePipelineContext ctx)
            {
                if (ctx.IsFluent)
                {
                    bool isCrit = false;
                    ctx.Mask |= isCrit ? DamageFlags.Critical : DamageFlags.None;
                }
            }
        }

        private class DamageFormulaStage : IDamageStage
        {
            public void Process(DamagePipelineContext ctx)
            {
                if (ctx.IsFluent)
                {
                    int damageValue = 100;

                    int critMultiplier;
                    if (ctx.IsCrit)
                    {
                        critMultiplier = 20000;
                    }
                    else
                    {
                        critMultiplier = 10000;
                    }

                    damageValue =(int)(damageValue * critMultiplier / 10000f);
                    ctx.BaseValue = damageValue;
                }
            }
        }

        private class ShieldCheckStage : IDamageStage
        {
            public void Process(DamagePipelineContext ctx)
            {
                if (ctx.IsFluent)
                {
                    int shieldValue = 60;
                    int finalValue = ctx.BaseValue - shieldValue;   // Max(0)
                    int absorbedValue = ctx.BaseValue - finalValue;
                    ctx.FinalValue = finalValue;
                    ctx.CostShieldValue = absorbedValue;

                    bool isAbsorbed = absorbedValue >= ctx.BaseValue;
                    ctx.Mask |= isAbsorbed ? DamageFlags.Absorbed : DamageFlags.None;
                }
            }
        }

        //private class LethalCheckStage : IDamageStage
        //{
        //    public void Process(DamagePipelineContext ctx)
        //    {
        //        if (ctx.IsAbsorbed)
        //        {
        //            return;
        //        }
        //    }
        //}

        private List<IDamageStage> stages;

        public DamagePipeline()
        {
            stages = new List<IDamageStage>();
            stages.Add(new HitCheckStage());
            stages.Add(new ImmunityCheckStage());
            stages.Add(new CritCheckStage());
            stages.Add(new DamageFormulaStage());
            stages.Add(new ShieldCheckStage());
        }

        public void Pipeline()
        {
            DamagePipelineContext ctx = new DamagePipelineContext();
            for (int i = 0; i < stages.Count; i++)
            {
                stages[i].Process(ctx);
            }
        }
    }
}
