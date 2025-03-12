using EC;
using Game.Cfg;
using Game.Core;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class CharacterComponent : EC.Component, IAwake
    {
        public void Awake()
        {
            Entity.AddComponent<QueueableComponent>();
            Entity.AddComponent<SceneActorComponent>();
            Entity.AddComponent<MotorComponent>();
        }

        public void Init(int id) 
        {
            var cc = World.GetComponent<ConfigComponent>();
            var cfg = cc.Find<RoleCfg>(id);

            Entity.GetComponent<SceneActorComponent>().Init(cfg.Actor);
            Entity.GetComponent<SceneActorComponent>().RefreshActor();
        }
    }
}
