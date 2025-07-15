using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GSystem 
{
    public class BattleTurnController
    {
        private BattleSystem system;

        private List<int> units;
        private Queue<int> readyStateUnits;
        private int actionUnit;

        public BattleTurnController(BattleSystem system) 
        {
            this.system = system;
            units = new List<int>();
            readyStateUnits = new Queue<int>();
            actionUnit = -1;
        }

        public void AddUnit(int unit) 
        {
            units.Add(unit);
        }

        public void Tick() 
        {
            if (actionUnit == -1) 
            {
                if (readyStateUnits.Count > 0) 
                {
                    actionUnit = readyStateUnits.Dequeue();
                    var unit = GetUnit(actionUnit);
                    var bbc = unit.GetComponent<BattleBehaviorComponent>();
                    bbc.AwaitBuffer();
                }
                else
                {
                    for (int i = 0; i < units.Count; i++)
                    {
                        var battleId = units[i];
                        var unit = GetUnit(battleId);
                        var bbc = unit.GetComponent<BattleBehaviorComponent>();
                        bbc.Step();
                        if (bbc.State == BehaviorState.Ready)
                        {
                            readyStateUnits.Enqueue(battleId);
                        }
                    }
                }
            }
            else
            {
                var unit = GetUnit(actionUnit);
                var bbc = unit.GetComponent<BattleBehaviorComponent>();
                if (bbc.State == BehaviorState.Prime)
                {
                    actionUnit = -1;
                }
            }
        }

        public void Submit(int x, int y) 
        {
            if (actionUnit == -1) 
            {
                return;
            }

            Game.UI.Back();

            var unit = GetUnit(actionUnit);
            var bbc = unit.GetComponent<BattleBehaviorComponent>();

            if (bbc.State == BehaviorState.MoveBuffer) 
            {
                SceneGridView view = system.GetBattleSceneView<SceneGridView>();
                var btc = unit.GetComponent<BattleTransformComponent>();

                List<Vector2Int> path = view.AStarPath(btc.pX, btc.pY, x, y);
                if (path.Count < 2)
                {
                    return;
                }

                List<Vector2Int> simple = BattleUtil.SimplifyPath(path);
                bbc.Move(simple);
            }

            if (bbc.State == BehaviorState.ActionBuffer) 
            {
                bbc.Action();
            }
        }

        private BattleUnit GetUnit(int battleId) 
        {
            return system.GetBattleUnit(battleId);
        }
    }
}