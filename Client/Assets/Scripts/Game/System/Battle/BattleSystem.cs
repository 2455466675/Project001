using Cysharp.Threading.Tasks;
using Game.UI.Input;
using Game.UI;
using System.Collections.Generic;

namespace Game.GSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class BattleSystem
    {
        public bool IsBattle { get; private set; }

        private List<BattleUnit> units;

        private Battle_Units_Group_Proxy proxy;

        private BattleTurnController turnController;

        public void Init() 
        {
            units = new List<BattleUnit>();

            for (int i = 0; i < 4; i++) 
            {
                BattleUnit unit = Game.System.UnitManager.CreateUnit<BattleUnit>();
                unit.AddComponent<BattleHeroComponent>();
                unit.Init(i);
                units.Add(unit);
            }

            for (int i = 0; i < 9; i++) 
            {
                BattleUnit unit = Game.System.UnitManager.CreateUnit<BattleUnit>();
                unit.AddComponent<BattleMonsterComponent>();
                unit.Init(i + 4);
                units.Add(unit);
            }

            turnController = new BattleTurnController(this);
        }

        public BattleUnit[] GetEnemies() 
        {
            return null;
        }

        public BattleUnit[] GetHeroes()
        {
            return null;
        }

        public BattleUnit GetBattleUnit(int battleId) 
        {
            return units.Find(u => u.BattleId == battleId);
        }

        public void OnSubmitGrid(int x, int y) 
        {
            turnController.Submit(x, y);
        }

        public void SetProxy(Battle_Units_Group_Proxy proxy) 
        {
            this.proxy = proxy;
        }
        
        public T GetBattleSceneView<T>() where T : View
        {
            if (proxy == null) 
            {
                return default;
            }
            return proxy.GetView<T>();
        }

        public void FixedUpdate(float fdt) 
        {
            if (!IsBattle) 
            {
                return;
            }
            turnController.Tick();
        }

        public void EnterBattle() 
        {
            Enter().Forget();
        }

        public void ExitBattle() 
        {
            for (int i = 0; i < units.Count; i++)
            {
                BattleUnit unit = units[i];
                unit.Clear();
            }

            Game.Scene.UnloadBattleScene();

            IsBattle = false;
        }

        private async UniTaskVoid Enter() 
        {
            await PlayTransitionAnim();
            await PreLoad();

            Game.Scene.LoadBattleScene();

            await EndLoad();

            await StopTransitionAnim();
            //await Appear();

            MLog.Log("Enter end");

            IsBattle = true;

            for (int i = 0; i < 2; i++)
            {
                var unit = GetBattleUnit(i);
                unit.GetComponent<ActorComponent>().SwitchAnimatorController(AnimatorControllerType.Battle);
                unit.GetComponent<ActorComponent>().SetRigidbodyEnable(false);
                unit.GetComponent<ActorComponent>().SetLocalRotation(70f);
                unit.GetComponent<BattleTransformComponent>().SetPosition(10 + i, 6 + i);
                unit.GetComponent<BattleAttributeComponent>().SetEffect(11101, i * 10000);
            }
        }

        private async UniTask PreLoad() 
        {
            //List<int> testMonster = new List<int>()
            //{
            //    300001, 0, 300002, 0, 300003, 0, 300004, 0, 300005
            //};

            //for (int i = 0; i < testMonster.Count; i++)
            //{
            //    BattleUnit unit = enemies[i];
            //    unit.Reset(testMonster[i]);
            //}

            List<int> testHero = new List<int>()
            {
                810001, 810002, //810003, 810004
            };

            for (int i = 0; i < testHero.Count; i++) 
            {
                BattleUnit unit = units[i];
                unit.Reset(testHero[i]);

                turnController.AddUnit(unit.BattleId);
            }

            await UniTask.Yield();
        }

        private async UniTask EndLoad()
        {

            var groupEntity = Game.UI.GetNavigationGroupEntity(UI.NavigationGroupDefine.Battle_Units_Group);
            groupEntity.Show();

            List<UniTask> tasks = new List<UniTask>();

            foreach (var unit in units)
            {
                UniTask task = unit.RefreshActorAsync();
                tasks.Add(task);
            }

            await UniTask.WhenAll(tasks);

            foreach (var unit in units)
            {
                unit.GetComponent<BattleHudComponent>().LoadHud();          
            }
        }

        private async UniTask PlayTransitionAnim() 
        {
            var e = Game.UI.GetNavigationGroupEntity(UI.NavigationGroupDefine.Battle_Loading_Group);
            e.Show();

            await GameMathf.Lerp(1f, -0.1f, 0.5f, (v) =>
            {
                Game.Event.Publish(new SceneLoadingProgressEventArgs() { progress = v });
            }, GameMathf.Easing.Linear);

            await UniTask.Yield();
        }

        private async UniTask StopTransitionAnim() 
        {
            await GameMathf.Lerp(-0.1f, 1f, 0.5f, (v) =>
            {
                Game.Event.Publish(new SceneLoadingProgressEventArgs() { progress = v });
            }, GameMathf.Easing.EaseInQuad);

            await UniTask.Yield();

            var e = Game.UI.GetNavigationGroupEntity(UI.NavigationGroupDefine.Battle_Loading_Group);
            e.Hide();
        }
    }
}
