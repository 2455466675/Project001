using Game.Cfg;
using Game.Core;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class BattleMonsterComponent : EC.Component
    {
        public void Init(int id) 
        {
            ConfigComponent cc = MyWorld.GetComponent<ConfigComponent>();
            MonsterCfg cfg = cc.Find<MonsterCfg>(id);

            if (cfg == null) 
            {
                return;
            }

            BattleActorComponent bac = GetComponent<BattleActorComponent>();
            bac.Init(cfg.Actor);
            bac.RefreshActor();
        }
    }
}
