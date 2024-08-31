
namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class GameSystem
    {
        public GameSystemConfig Config { get; private set; }

        public GameGm Gm { get; private set; }

        public ItemSystem ItemSystem { get; private set; }

        public RoleSystem RoleSystem { get; private set; }

        public FightSystem FightSystem { get; private set; }

        public OverviewSystem OverviewSystem { get; private set; }

        public GameSystem()
        {
            Config = GameCore.ResourceManager.LoadAsset<GameSystemConfig>(GameCore.GameCfg.Formula.GAME_SYSTEM_CONFIG_PATH);

            Gm = new GameGm();
            ItemSystem = new ItemSystem();
            RoleSystem = new RoleSystem();
            FightSystem = new FightSystem();
            OverviewSystem = new OverviewSystem();
        }
    }
}