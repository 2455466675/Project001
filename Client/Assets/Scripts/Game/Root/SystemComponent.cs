using Game.System;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    public class SystemComponent : ECS.Entity
    {
        public LoginSystem LoginSystem { get; private set; }

        public void Init() 
        {
            LoginSystem = AddComponent<LoginSystem>();
        }
    }
}