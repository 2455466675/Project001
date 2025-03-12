using EC;
using Game.Core;
using Game.UI;
using System.Collections.Generic;

namespace Game.System
{
    public class BattleUnit : EC.Component, IAwake
    {
        public int UnitId { get; private set; }

        public BattleActorComponent ActorComponent { get; private set; }

        public void Awake()
        {
            ActorComponent = Entity.AddComponent<BattleActorComponent>();
        }

        public void SetUnitId(int unitId) 
        {
            UnitId = unitId;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BattleComponent : EC.Component, IAwake
    {
        private RoundComponent roundComponent;

        private List<BattleUnit> playerUnits;
        private List<BattleUnit> enemyUnits;

        public void Awake()
        {
            roundComponent = Entity.AddComponent<RoundComponent>();

            playerUnits = new List<BattleUnit>();
            enemyUnits = new List<BattleUnit>();      
        }

        public void Enter() 
        {
            World.GetComponent<UIComponent>().Close(true);
            World.GetComponent<SceneComponent>().EnterBattleScene(LoadEndHandler);
        }

        public void Exit() 
        {
            World.GetComponent<UIComponent>().Close(true);
            World.GetComponent<SceneComponent>().ExitBattleScene();

            for (int i = 0; i < playerUnits.Count; i++)
            {
                World.DestroyEntity(playerUnits[i].Entity);
            }

            for (int i = 0; i < enemyUnits.Count; i++)
            {
                World.DestroyEntity(enemyUnits[i].Entity);
            }

            playerUnits.Clear();
            enemyUnits.Clear();
        }

        public BattleUnit GetUnit(int unitId) 
        {
            BattleUnit unit = playerUnits.Find(u => u.UnitId == unitId);
            unit ??= enemyUnits.Find(u => u.UnitId == unitId);
            return unit;
        }

        private void LoadEndHandler(SceneEntity info)
        {
            StaticNavigationGroup playerGroup = NavigationGroup.GetNavigationGroup(UIDefine.Group_ID.Battle_Player_Group) as StaticNavigationGroup;
            StaticNavigationGroup enemyGroup = NavigationGroup.GetNavigationGroup(UIDefine.Group_ID.Battle_Enemy_Group) as StaticNavigationGroup;

            int index = -1;

            for (int i = 0; i < 4; i++)
            {
                BattlePointItem point = playerGroup.GetItem(i) as BattlePointItem;

                Entity entity = Entity.CreateChild();
                var unit = entity.AddComponent<BattleUnit>();
                entity.AddComponent<BattleHeroComponent>();
                
                index ++;
                point.SetUnitId(index);
                unit.SetUnitId(index);
                unit.ActorComponent.SetBattlePointItem(point);

                playerUnits.Add(unit);
            }

            for (int i = 0; i < 9; i++)
            {
                BattlePointItem point = enemyGroup.GetItem(i) as BattlePointItem;

                Entity entity = Entity.CreateChild();
                var unit = entity.AddComponent<BattleUnit>();
                entity.AddComponent<BattleMonsterComponent>();

                index ++;
                point.SetUnitId(index);
                unit.SetUnitId(index);
                unit.ActorComponent.SetBattlePointItem(point);

                enemyUnits.Add(unit);
            }

            int[] enemies = new int[9]
            {
                300001, 0, 300003, 0, 300002, 0, 300004, 0, 300005,
            };

            for (int i = 0; i < enemyUnits.Count; i++)
            {
                BattleUnit unit = enemyUnits[i];
                BattleMonsterComponent bmc = unit.GetComponent<BattleMonsterComponent>();
                bmc.Init(enemies[i]);
            }

            World.GetComponent<UIComponent>().Navigate(UIDefine.Group_ID.Battle_Enemy_Group);
            //roundComponent.Start();
        }
    }
}
