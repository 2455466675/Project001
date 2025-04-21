
namespace Game.System
{
    public class GameSystem
    {
        public InventorySystem InventorySystem { get; private set; }
        public LoginSystem LoginSystem { get; private set; }
        public RoleSystem RoleSystem { get; private set; }
        public PartySystem PartySystem { get; private set; }

        public BackpackSystem BackpackSystem { get; private set; }
        public OverviewSystem OverviewSystem { get; private set; }

        public void Init(GameInitConfig config)
        {
            InventorySystem = new InventorySystem();
            InventorySystem.Init();

            LoginSystem = new LoginSystem();
            LoginSystem.Init();

            RoleSystem = new RoleSystem();
            RoleSystem.Init(config);

            PartySystem = new PartySystem();
            PartySystem.Init();

            BackpackSystem = new BackpackSystem();
            BackpackSystem.Init();

            OverviewSystem = new OverviewSystem();
            OverviewSystem.Init();

            InventorySystem.Test();
        }

        public void FixedUpdate(float fdt) 
        {
            RoleSystem.FixedUpdate(fdt);
        }
    }
}