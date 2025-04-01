using ECS;
using System.Collections;
using System.Collections.Generic;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class RoleSystem : Entity
    {
        private ActorContainer container;

        public void Init(GameInitConfig config)
        {
            var go = GameWorld.Root.GetComponent<ResourceComponent>().LoadAndInstantiate(config.ActorContainerPath, GameWorld.GameWorldObject.transform);
            container = go.GetComponent<ActorContainer>();

            CharacterEntity character = CreateChild<CharacterEntity>();
            character.AddComponent<ActorComponent>();
        }
    }
}
