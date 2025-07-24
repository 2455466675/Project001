using Cysharp.Threading.Tasks;
using Game.UI;
using Game.UI.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GSystem
{
    public class SelectMoveTargetCammand : InputCammand
    {
        protected override bool OnPush()
        {
            return Execute();
        }

        protected override bool OnRise()
        {
            return Execute();
        }

        protected override void OnSink()
        {
            Pop();
            base.OnSink();
        }

        private bool Execute()
        {
            int index = BattleUtil.IndexToIndex(10, 6);
            NavigationListCammand listCammand = new NavigationListCammand(NavigationListDefine.Battle_Grid, new int[] { index });

            return Push(listCammand);
        }

        protected override bool CheckIsLocked()
        {
            return true;
        }
    }

    public class UnitMoveCammand : InputCammand
    {
        private List<Vector2Int> path;

        public UnitMoveCammand(List<Vector2Int> path) : base()
        {
            this.path = path;
        }

        protected override bool OnPush()
        {
            Move().Forget();
            return base.OnPush();
        }

        protected override void OnPop()
        {
            var unit = Game.System.BattleSystem.GetBattleUnit(0);

            var btc = unit.GetComponent<BattleTransformComponent>();
            btc.SetPosition(10, 6);

            base.OnPop();
        }

        private async UniTask Move() 
        {
            var unit = Game.System.BattleSystem.GetBattleUnit(0);

            var btc = unit.GetComponent<BattleTransformComponent>();

            await btc.MoveAsync(path);
        }
    }

    public class BattleTurnController
    {
        private BattleSystem system;

        private List<BattleBehaviorComponent> units;
        private Queue<int> readyUnits;
        public int ActioningBattleId { get; private set; }

        public BattleTurnController(BattleSystem system) 
        {
            this.system = system;

            readyUnits = new Queue<int>();

            units = new List<BattleBehaviorComponent>();
        }

        public void AddUnit(BattleUnit unit)
        {
            BattleBehaviorComponent component = unit.GetComponent<BattleBehaviorComponent>();

            units.Add(component);

            component.Attach(this);
        }

        public void EnterReady(int battleId) 
        {
            readyUnits.Enqueue(battleId);
        }

        public void ExitAction(int battleId)
        {
            if (ActioningBattleId != battleId)
            {
                MLog.Error("EnterAction error:" + ActioningBattleId);
                return;
            }

            ActioningBattleId = -1;
        }

        public void StartUp() 
        {
            ActioningBattleId = -1;

            for (int i = 0; i < units.Count; i++)
            {
                units[i].StartUp();
            }
        }

        public void Tick() 
        {
            CheckActioningUnit();
            TickUnits(); 
        }

        private void CheckActioningUnit() 
        {
            if (ActioningBattleId >= 0) 
            {
                return;
            }

            if (readyUnits.Count == 0) 
            {
                return;
            }

            ActioningBattleId = readyUnits.Dequeue();
        }

        private void TickUnits() 
        {
            for (int i = 0; i < units.Count; i++)
            {
                units[i].Tick();
            }
        }

        public void Submit(int x, int y) 
        {
            if (ActioningBattleId == -1)
            {
                return;
            }

            var unit = GetUnit(0);

            SceneGridView view = system.GetBattleSceneView<SceneGridView>();
            var btc = unit.GetComponent<BattleTransformComponent>();

            List<Vector2Int> path = view.AStarPath(btc.pX, btc.pY, x, y);
            if (path.Count < 2)
            {
                return;
            }

            List<Vector2Int> simple = BattleUtil.SimplifyPath(path);
            BattleInputModule.Instance.Push(new UnitMoveCammand(simple));

            //Game.UI.Back();

            //var unit = GetUnit(ActioningBattleId);
            //var bbc = unit.GetComponent<BattleBehaviorComponent>();

            //if (bbc.step == 0)
            //{
            //    SceneGridView view = system.GetBattleSceneView<SceneGridView>();
            //    var btc = unit.GetComponent<BattleTransformComponent>();

            //    List<Vector2Int> path = view.AStarPath(btc.pX, btc.pY, x, y);
            //    if (path.Count < 2)
            //    {
            //        return;
            //    }

            //    List<Vector2Int> simple = BattleUtil.SimplifyPath(path);
            //    bbc.Move(simple);
            //}

            //if (bbc.step == 1)
            //{
            //    bbc.Action();
            //}
        }

        private BattleUnit GetUnit(int battleId) 
        {
            return system.GetBattleUnit(battleId);
        }
    }
}