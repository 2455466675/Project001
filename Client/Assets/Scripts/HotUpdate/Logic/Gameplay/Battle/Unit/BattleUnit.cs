using Cysharp.Threading.Tasks;
using ECS;
using GameFramework.Utility.GameDefine;
using System.Collections.Generic;

namespace GameFramework.Logic
{
    public class BattleUnitComponent : ComponentBase
    {
        public int BattleId { get; set; }
        public int Index { get; set; }
        public BattleCamp Camp { get; set; }
    }

    public class BattleUnit
    {
        private static int BattleIdCounter = 1000;
        private Dictionary<int, Entity> entities;

        public void CreateUnit(List<int> unit)
        {
            entities = new Dictionary<int, Entity>();

            unit ??= new List<int> { 0, 1, 0, 0, 1, 0 };
            for (int i = 0; i < unit.Count; i++)
            {
                int id = unit[i];
                if (id != 0)
                {
                    int battleId = ++BattleIdCounter;
                    var entity = Game.GetSystem<GameEntityFactory>().CreateEntity();
                    var buc = entity.AddComponent<BattleUnitComponent>();
                    buc.BattleId = battleId;
                    buc.Index = i;
                    buc.Camp = BattleCamp.Enemy;

                    var ac = entity.AddComponent<ActorComponent>();
                    ac.SetActorId(id);
                    ac.SetActorType(ActorType.Party);
                    ac.SetAnimatorController("battle");

                    var pc = entity.AddComponent<PropertyComponent>();
                    pc.CreateProperty(PropertyDefine.MaxHp, 100);
                    pc.CreateProperty(PropertyDefine.CurHp, 100);
                    pc.CreateProperty(PropertyDefine.P_ATK, 10);
                    pc.CreateProperty(PropertyDefine.P_DEF, 8);
                    pc.CreateProperty(PropertyDefine.Speed, 5);
                    pc.CreateProperty(PropertyDefine.CriticalRate, 500);
                    pc.CreateProperty(PropertyDefine.CriticalDamage, 15000);
                    pc.CreateProperty(PropertyDefine.HitRate, 8000);
                    pc.CreateProperty(PropertyDefine.DodgeRate, 1000);

                    entities.Add(battleId, entity);
                }
            }

            var players = Game.GetModule<PartyModule>().GetBattleUnits();
            for (int i = 0; i < players.Count; i++)
            {
                int id = players[i];
                if (id != 0)
                {
                    int battleId = ++BattleIdCounter;
                    var entity = Game.GetSystem<GameEntityFactory>().CreateEntity();
                    var buc = entity.AddComponent<BattleUnitComponent>();
                    buc.BattleId = battleId;
                    buc.Index = i;
                    buc.Camp = BattleCamp.Player;

                    var ac = entity.AddComponent<ActorComponent>();
                    ac.SetActorId(id);
                    ac.SetActorType(ActorType.Party);
                    ac.SetAnimatorController("battle");

                    var pc = entity.AddComponent<PropertyComponent>();
                    pc.CreateProperty(PropertyDefine.MaxHp, 100);
                    pc.CreateProperty(PropertyDefine.CurHp, 100);
                    pc.CreateProperty(PropertyDefine.P_ATK, 12);
                    pc.CreateProperty(PropertyDefine.P_DEF, 5);
                    pc.CreateProperty(PropertyDefine.Speed, 6);
                    pc.CreateProperty(PropertyDefine.CriticalRate, 500);
                    pc.CreateProperty(PropertyDefine.CriticalDamage, 15000);
                    pc.CreateProperty(PropertyDefine.HitRate, 9000);
                    pc.CreateProperty(PropertyDefine.DodgeRate, 3000);

                    entities.Add(battleId, entity);
                }
            }
        }

        public async UniTask RefreshUnit()
        {
            var formation = Game.GetModule<BattleModule>().Formation;

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];

                var buc = entity.GetComponent<BattleUnitComponent>();
                var site = formation.GetFormationSite(buc.Camp, buc.Index);

                var ac = entity.GetComponent<ActorComponent>();
                ac.SetVisible(true);
                ac.SetPosition(site.Position);
                await ac.RefreshActor();
            }
        }

        public Entity GetBattleUnit(int battleId)
        {
            entities.TryGetValue(battleId, out var entity);
            return entity;
        }
    }
}
