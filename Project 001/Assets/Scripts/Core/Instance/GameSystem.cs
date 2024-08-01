
namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class GameSystem
    {
        public GameSystemConfig Config { get; private set; }

        public ItemSystem ItemSystem { get; private set; }

        public RoleSystem RoleSystem { get; private set; }

        public OverviewSystem OverviewSystem { get; private set; }

        public GameSystem()
        {
            Config = GameCore.ResourceManager.LoadAsset<GameSystemConfig>(GameCore.GameCfg.Formula.GAME_SYSTEM_CONFIG_PATH);

            ItemSystem = new ItemSystem();
            RoleSystem = new RoleSystem();
            OverviewSystem = new OverviewSystem();
        }
    }
}