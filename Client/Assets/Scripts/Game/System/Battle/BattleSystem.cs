using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Game.GSystem
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

        public BattleUnit[] GetHeroes()
        {
            return heroes.ToArray();
        }

        public void EnterBattle() 
        {
            Enter().Forget();
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

        private async UniTaskVoid Enter() 
        {
            await PreLoad();

            Game.Scene.LoadBattleScene();

            await EndLoad();

            await Appear();

            MLog.Log("Enter end");
        }

        private async UniTask PreLoad() 
        {
            await PlayTransitionAnim();

            List<int> testMonster = new List<int>()
            {
                300001, 0, 300002, 0, 300003, 0, 300004, 0, 300005
            };

            for (int i = 0; i < testMonster.Count; i++)
            {
                BattleUnit unit = enemies[i];
                unit.Reset(testMonster[i]);
            }

            List<int> testHero = new List<int>()
            {
                810001, 810002, 810003, 810004
            };

            for (int i = 0;i < testHero.Count; i++) 
            {
                BattleUnit unit = heroes[i];
                unit.Reset(testHero[i]);
            }
        }

        private async UniTask EndLoad()
        {

            var groupEntity = Game.UI.GetNavigationGroupEntity(UI.NavigationGroupDefine.Battle_Units_Group);
            groupEntity.Show();

            List<UniTask> tasks = new List<UniTask>();

            foreach (var unit in enemies)
            {
                UniTask task = unit.RefreshActorAsync();
                tasks.Add(task);
            }

            foreach (var unit in heroes)
            {
                UniTask task = unit.RefreshActorAsync();
                tasks.Add(task);
            }

            await UniTask.WhenAll(tasks);

            await StopTransitionAnim();
        }

        private async UniTask PlayTransitionAnim() 
        {
            var e = Game.UI.GetNavigationGroupEntity(UI.NavigationGroupDefine.Battle_Loading_Group);
            e.Show();

            await GameMathf.Lerp(1f, -0.1f, 0.5f, (v) =>
            {
                Game.Event.Publish(new SceneLoadingProgress() { progress = v });
            }, GameMathf.Easing.Linear);

            await UniTask.Yield();
        }

        private async UniTask StopTransitionAnim() 
        {
            await GameMathf.Lerp(-0.1f, 1f, 0.5f, (v) =>
            {
                Game.Event.Publish(new SceneLoadingProgress() { progress = v });
            }, GameMathf.Easing.EaseInQuad);

            await UniTask.Yield();

            var e = Game.UI.GetNavigationGroupEntity(UI.NavigationGroupDefine.Battle_Loading_Group);
            e.Hide();
        }

        private async UniTask Appear() 
        {
            List<UniTask> tasks = new List<UniTask>();

            foreach (var unit in heroes)
            {
                ActionHandle handle = unit.PlayAction(Common.StringToHash("Appear"));
                tasks.Add(handle.Task);
            }

            await UniTask.WhenAll(tasks);
        }
    }
}
