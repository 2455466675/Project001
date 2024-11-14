using MVC;

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

        public BattleSystem BattleSystem { get; private set; }

        public OverviewSystem OverviewSystem { get; private set; }
      
        public GameSystem()
        {
            DataContainer.CreateRoot();
            DataContainer GameData = DataContainer.Root.CreateContainer("Game");

            Config = GameCore.ResourceManager.LoadAsset<GameSystemConfig>(GameCore.Cfg.Formula.GAME_SYSTEM_CONFIG_PATH);

            Gm = new GameGm(GameData.CreateContainer("GM"));
            ItemSystem = new ItemSystem(GameData.CreateContainer("Item"));
            RoleSystem = new RoleSystem(GameData.CreateContainer("Role"));
            BattleSystem = new BattleSystem(GameData.CreateContainer("Battle"));
            OverviewSystem = new OverviewSystem(GameData.CreateContainer("Overview"));          
        }
    }
}