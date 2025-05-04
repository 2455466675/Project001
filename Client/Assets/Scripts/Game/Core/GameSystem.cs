
namespace Game.System
{
    public class GameSystem
    {
        public UnitManager UnitManager { get; private set; }
        public ActorManager ActorManager { get; private set; }
        public InventorySystem InventorySystem { get; private set; }
        public LoginSystem LoginSystem { get; private set; }
        public PartySystem PartySystem { get; private set; }
        public BackpackSystem BackpackSystem { get; private set; }
        public OverviewSystem OverviewSystem { get; private set; }

        public BattleSystem BattleSystem { get; private set; }

        public void Init()
        {
            UnitManager = new UnitManager();
            UnitManager.Init();

            ActorManager = new ActorManager();
            ActorManager.Init();

            InventorySystem = new InventorySystem();
            InventorySystem.Init();

            LoginSystem = new LoginSystem();
            LoginSystem.Init();

            PartySystem = new PartySystem();
            PartySystem.Init();

            BackpackSystem = new BackpackSystem();
            BackpackSystem.Init();

            OverviewSystem = new OverviewSystem();
            OverviewSystem.Init();

            BattleSystem = new BattleSystem();
            BattleSystem.Init();

            InventorySystem.Test();
        }

        public void FixedUpdate(float fdt) 
        {
            UnitManager.FixedUpdate(fdt);
        }
    }
}