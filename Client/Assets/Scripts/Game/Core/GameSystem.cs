
namespace Game.System
{
    public class GameSystem
    {
        public LoginSystem LoginSystem { get; private set; }
        public RoleSystem RoleSystem { get; private set; }

        public void Init(GameInitConfig config)
        {
            LoginSystem = new LoginSystem();
            RoleSystem = new RoleSystem();
            RoleSystem.Init(config);
        }

        public void FixedUpdate(float fdt) 
        {
            RoleSystem.FixedUpdate(fdt);
        }
    }
}