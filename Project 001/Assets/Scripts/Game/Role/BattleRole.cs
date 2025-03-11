using MVC;

namespace Game.System
{
    public enum BattleRoleType
    {
        Enemy,
        Player,
    }

    /// <summary>
    /// 
    /// </summary>
	public abstract class BattleRole : RoleBase
    {
        /// <summary>
        /// 位置编号
        /// </summary>
        public int Index { get; protected set; }
        public BattlePointItem FightPoint { get; protected set; }

        public virtual BattleRoleType FightCharacterType { get;}

        public virtual string ActorPath { get; }

        public BattleRole(int index, DataContainer container) : base(container)
        {
            Index = index;
            SetBaseValue("index", index);
            SetBaseValue("valid", false);
        }

        /// <summary>
        /// 重置此位置
        /// </summary>
        public abstract void Reset();

        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="cfgId"></param>
        public abstract void UpdateCfg(int cfgId);

        public override void RefreshActor()
        {
        }
    }
}

