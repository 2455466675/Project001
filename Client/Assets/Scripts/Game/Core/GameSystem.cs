
namespace Game.System
{
    public class GameSystem
    {
        public LoginSystem LoginSystem { get; private set; }
        public RoleSystem RoleSystem { get; private set; }

        public PartySystem PartySystem { get; private set; }

        public void Init(GameInitConfig config)
        {
            LoginSystem = new LoginSystem();
            RoleSystem = new RoleSystem();
            RoleSystem.Init(config);

            PartySystem = new PartySystem();
            PartySystem.Init();
        }

        public void FixedUpdate(float fdt) 
        {
            RoleSystem.FixedUpdate(fdt);
        }
    }
}