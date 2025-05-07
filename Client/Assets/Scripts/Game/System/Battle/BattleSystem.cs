using System.Collections;
using System.Collections.Generic;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class BattleSystem
    {
        private List<BattleUnit> heroes;
        private List<BattleUnit> enemies;

        public void Init() 
        {
            enemies = new List<BattleUnit>();
            heroes = new List<BattleUnit>();

            for (int i = 0; i < 4; i++) 
            {
                BattleUnit unit = Game.System.UnitManager.CreateUnit<BattleUnit>();
                unit.AddComponent<BattleHeroComponent>();
                unit.Init(i);
                heroes.Add(unit);
            }

            for (int i = 0; i < 9; i++) 
            {
                BattleUnit unit = Game.System.UnitManager.CreateUnit<BattleUnit>();
                unit.AddComponent<BattleMonsterComponent>();
                unit.Init(i + 4);
                enemies.Add(unit);
            }
        }

        public BattleUnit[] GetEnemies() 
        {
            return enemies.ToArray();
        }

        public void EnterBattle() 
        {
            Game.Scene.LoadBattleScene(OnEnterScene);
        }

        public void ExitBattle() 
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                BattleUnit unit = enemies[i];
                unit.Clear();
            }

            Game.Scene.UnloadBattleScene();
        }

        private void OnEnterScene() 
        {
            MLog.Log("OnEnterScene");

            List<int> testMonster = new List<int>()
            {
                300001, 0, 300002, 0, 300003, 0, 300004, 0, 300005
            };

            for (int i = 0; i < testMonster.Count; i++) 
            {
                BattleUnit unit = enemies[i];
                unit.Reset(testMonster[i]);
            }

            Game.UI.Navigate(UI.NavigationListDefine.Battle_Enemy_Unit_List, UI.Input.ModuleType.Battle);
        }
    }
}
