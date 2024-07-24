
using Unity.VisualScripting;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class GameSystem
    {
        public GameSystemConfig Config { get; private set; }
        public static GameSystem Inst { get; private set; }

        public ItemSystem ItemSystem { get; private set; }

        public RoleSystem RoleSystem { get; private set; }

        public GameSystem()
        {
            Config = GameCore.ResourceManager.LoadAsset<GameSystemConfig>("Assets/Bundles/Common/GameSystemConfig");

            Inst = this;
            ItemSystem = new ItemSystem();
            RoleSystem = new RoleSystem();

        }
    }
}