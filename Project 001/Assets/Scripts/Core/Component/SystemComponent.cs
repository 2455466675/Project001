using EC;
using Game.Core;
using System.Collections;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemComponent : EC.Component, IInitializable
    {
        public GameSystemConfig Config { get; private set; }
    
        public OverviewComponent OverviewComponent { get; private set; }
        public PartyComponent PartyComponent { get; private set; }
        public PackageComponent PackageComponent { get; private set; }
        public BattleComponent BattleComponent { get; private set; }

        public IEnumerator Init(GameInitCfg intCfg)
        {
            ResourceComponent rc = MyWorld.GetComponent<ResourceComponent>();

            Config = rc.LoadAsset<GameSystemConfig>(MyWorld.GetComponent<ConfigComponent>().Formula.GAME_SYSTEM_CONFIG_PATH);

            Entity systemEntity = MyEntity.CreateChild();

            OverviewComponent = systemEntity.CreateChild().AddComponent<OverviewComponent>();

            PartyComponent = systemEntity.CreateChild().AddComponent<PartyComponent>();

            PackageComponent = systemEntity.CreateChild().AddComponent<PackageComponent>();

            BattleComponent = systemEntity.CreateChild().AddComponent<BattleComponent>();

            yield return null;
        }
    }
}
