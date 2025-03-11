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
            MyEntity.AddComponent<QueueableComponent>();
            MyEntity.AddComponent<SceneActorComponent>();
            MyEntity.AddComponent<MotorComponent>();
        }

        public void Init(int id) 
        {
            var cc = MyWorld.GetComponent<ConfigComponent>();
            var cfg = cc.Find<RoleCfg>(id);

            MyEntity.GetComponent<SceneActorComponent>().Init(cfg.Actor);
            MyEntity.GetComponent<SceneActorComponent>().RefreshActor();
        }
    }
}
