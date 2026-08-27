namespace GameFramework.Logic
{
    public enum TargetRule
    {
        None = 0,
        SelectOneEnemy = 1,
        /// <summary>
        /// 随机一个敌人（只随机一次，多段唯一）
        /// </summary>
        RandomOneEnemy_a = 2,
        /// <summary>
        /// 随机一个敌人（每段都随机一次）
        /// </summary>
        RandomOneEnemy_b = 3,
        /// <summary>
        /// 所有敌人
        /// </summary>
        AllEnemys = 4,
        /// <summary>
        /// 指定一个友军（包含自己）（包含倒地）
        /// </summary>
        SelectOneAlly_a = 5,
        /// <summary>
        /// 指定一个友军（不包含自己）（包含倒地）
        /// </summary>
        SelectOneAlly_b = 6,
        /// <summary>
        /// 指定一个友军（包含自己）（不包含倒地）
        /// </summary>
        SelectOneAlly_c = 7,
        /// <summary>
        /// 指定一个友军（不包含自己）（不包含倒地）
        /// </summary>
        SelectOneAlly_d = 8,
        /// <summary>
        /// 全部友军（包含自己）（包含倒地）
        /// </summary>
        AllAlly_a = 9,
        /// <summary>
        /// 全部友军（不包含自己）（包含倒地）
        /// </summary>
        AllAlly_b = 10,
        /// <summary>
        /// 全部友军（包含自己）（不包含倒地）
        /// </summary>
        AllAlly_c = 11,
        /// <summary>
        /// 全部友军（不包含自己）（不包含倒地）
        /// </summary>
        AllAlly_d = 12,
        /// <summary>
        /// 随机一个友军（包含自己）（包含倒地）
        /// </summary>
        RandomOnAlly_a = 13,
        /// <summary>
        /// 随机一个友军（不包含自己）（包含倒地）
        /// </summary>
        RandomOnAlly_b = 14,
        /// <summary>
        /// 随机一个友军（包含自己）（不包含倒地）
        /// </summary>
        RandomOnAlly_c = 15,
        /// <summary>
        /// 随机一个友军（不包含自己）（不包含倒地）
        /// </summary>
        RandomOnAlly_d = 16,
        /// <summary>
        /// 所有人（包含自己）（包含倒地）
        /// </summary>
        AllUnit_a = 17,
        /// <summary>
        /// 所有人（包含自己）（不包含倒地）
        /// </summary>
        AllUnit_b = 18,
        /// <summary>
        /// 所有人（不包含自己）（包含倒地）
        /// </summary>
        AllUnit_c = 19,
        /// <summary>
        /// 所有人（不包含自己）（不包含倒地）
        /// </summary>
        AllUnit_d = 20,
        /// <summary>
        /// 随机一个人（包含自己）（包含倒地）
        /// </summary>
        RandomOneUnit_a = 21,
        /// <summary>
        /// 随机一个人（包含自己）（不包含倒地）
        /// </summary>
        RandomOneUnit_b = 22,
        /// <summary>
        /// 随机一个人（不包含自己）（包含倒地）
        /// </summary>
        RandomOneUnit_c = 23,
        /// <summary>
        /// 随机一个人（不包含自己）（不包含倒地）
        /// </summary>
        RandomOneUnit_d = 24,
    }

    public enum StrikeRange
    {
        None = 0,
        /// <summary>
        /// 所有选定的目标
        /// </summary>
        AllTargets = 1,
        /// <summary>
        /// 从选定目标中随机一个
        /// </summary>
        RandomInTargets = 2,
        /// <summary>
        /// 所有敌方单位
        /// </summary>
        AllEnemys = 3,
        /// <summary>
        /// 施法者
        /// </summary>
        Caster = 4,
    }

    public enum FormulaType
    {
        None = 0,
        Damage  = 1,
        AddBuff = 2,
    }

    public enum DamageFormula
    {
        None = 0,
        Formula_1 = 1,
    }
}
