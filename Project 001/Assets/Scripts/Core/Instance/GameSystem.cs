
namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class GameSystem
    {
        public static GameSystem Inst { get; private set; }

        public ItemSystem ItemSystem { get; private set; }

        public RoleSystem RoleSystem { get; private set; }

        public GameSystem()
        {
            Inst = this;
            ItemSystem = new ItemSystem();
            RoleSystem = new RoleSystem();
        }
    }
}