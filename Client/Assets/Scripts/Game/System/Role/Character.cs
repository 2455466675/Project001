using Config;
using System.Collections;
using System.Collections.Generic;

namespace Game.System
{
    public class Character
    {
        private ActorComponent actorComponent;

        public Character() 
        {
            actorComponent = new ActorComponent();
        }

        public void Init(int id) 
        {
            HeroCfg cfg = Game.Config.Find<HeroCfg>(id);
            actorComponent.Init(cfg.ActorId);
            actorComponent.Refresh();
        }

        public void Move(float x, float y) 
        {
            actorComponent.Move(x, y);
        }

        public void Run(bool isRunning)
        {
            actorComponent.Run(isRunning);
        }
    }
}