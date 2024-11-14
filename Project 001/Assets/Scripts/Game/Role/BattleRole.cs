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

        public DataContainer CharacterData { get; private set; }

        public BattleRole(int index, DataContainer container) : base(container)
        {
            Index = index;
            SetBaseValue("id", index);
            SetBaseValue("valid", false);

            CharacterData = CreateContainer("CharacterData");

            CharacterData.SetBaseValue("hp", UnityEngine.Random.Range(10, 100));
            CharacterData.SetBaseValue("maxHp", UnityEngine.Random.Range(100, 150));
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
            Actor = FightPoint.actorLoader.LoadActor(ActorPath);
            SetBaseValue("valid", Actor != null);
        }
    }
}

