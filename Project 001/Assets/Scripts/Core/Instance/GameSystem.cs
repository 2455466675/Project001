
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

            DataContainer GameData = DataContainer.Root.CreateContainer("Game");

            DataContainer package = GameData.CreateContainer("Package");
            DataCollection itemList = package.CreateCollection("ItemList");
            for (int i = 0; i < 20; i++)
            {
                DataContainer item = itemList.Append(true);
                item.SetBaseValue("id", i + 1);
                item.SetBaseValue("name", $"{i + 1}--item");
                item.SetBaseValue("count", i * 2);
            }

            DataContainer overview = GameData.CreateContainer("Overview");
            DataCollection menuList = overview.CreateCollection("menuList");
            for (int i = 0; i < 6; i++)
            {
                DataContainer item = menuList.Append(true);
                item.SetBaseValue("id", 910005 + i);
            }
        }
    }
}