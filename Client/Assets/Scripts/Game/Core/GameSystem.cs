
namespace Game.System
{
    public class GameSystem
    {
        public InventorySystem InventorySystem { get; private set; }
        public LoginSystem LoginSystem { get; private set; }
        public RoleSystem RoleSystem { get; private set; }
        public PartySystem PartySystem { get; private set; }
        public OverviewSystem OverviewSystem { get; private set; }

        public void Init(GameInitConfig config)
        {
            InventorySystem = new InventorySystem();

            LoginSystem = new LoginSystem();
            LoginSystem.Init();

            RoleSystem = new RoleSystem();
            RoleSystem.Init(config);

            PartySystem = new PartySystem();
            PartySystem.Init();

            OverviewSystem = new OverviewSystem();
            OverviewSystem.Init();
        }

        public void FixedUpdate(float fdt) 
        {
            RoleSystem.FixedUpdate(fdt);
        }
    }
}