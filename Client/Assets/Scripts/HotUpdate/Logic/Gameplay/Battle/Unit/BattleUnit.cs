using Cysharp.Threading.Tasks;
using ECS;
using System.Collections.Generic;

namespace GameFramework.Logic
{
    public class BattleUnitComponent : ComponentBase
    {
        public int Index { get; set; }
        public BattleCamp Camp { get; set; }
    }


    public class BattleUnit
    {
        private List<Entity> entities;

        public void CreateUnit(List<int> unit)
        {
            unit ??= new List<int> { 0, 1, 0, 0, 1, 0 };

            entities = new List<Entity>();

            for (int i = 0; i < unit.Count; i++)
            {
                int id = unit[i];
                if (id != 0)
                {
                    var entity = Game.GetSystem<GameEntityFactory>().CreateEntity();
                    var buc = entity.AddComponent<BattleUnitComponent>();
                    buc.Index = i;
                    buc.Camp = BattleCamp.Enemy;

                    var ac = entity.AddComponent<ActorComponent>();
                    ac.SetActorId(id);
                    ac.SetActorType(ActorType.Party);
                    ac.SetAnimatorController("battle");

                    entities.Add(entity);
                }
            }

            var players = Game.GetModule<PartyModule>().GetBattleUnits();
            for (int i = 0; i < players.Count; i++)
            {
                int id = players[i];
                if (id != 0)
                {
                    var entity = Game.GetSystem<GameEntityFactory>().CreateEntity();
                    var buc = entity.AddComponent<BattleUnitComponent>();
                    buc.Index = i;
                    buc.Camp = BattleCamp.Player;

                    var ac = entity.AddComponent<ActorComponent>();
                    ac.SetActorId(id);
                    ac.SetActorType(ActorType.Party);
                    ac.SetAnimatorController("battle");

                    entities.Add(entity);
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
    }
}
