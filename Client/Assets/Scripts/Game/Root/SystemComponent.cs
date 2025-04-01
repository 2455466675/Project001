using Game.System;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    public class SystemComponent : ECS.Entity
    {
        public LoginSystem LoginSystem { get; private set; }
        public RoleSystem RoleSystem { get; private set; }

        public void Init(GameInitConfig config) 
        {
            LoginSystem = AddComponent<LoginSystem>();
            RoleSystem = AddComponent<RoleSystem>();
            RoleSystem.Init(config);
        }
    }
}