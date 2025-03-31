using ECS;
using System.Collections;
using System.Collections.Generic;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class RoleSystem : Entity, IAwake
    {
        public void Awake()
        {
            CharacterEntity character = CreateChild<CharacterEntity>();
            character.AddComponent<ActorComponent>();
        }
    }
}
