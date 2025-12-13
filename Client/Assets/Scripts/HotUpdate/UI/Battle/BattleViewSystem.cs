using GameFramework.Core;
using GameFramework.Gameplay;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.UI
{
    public abstract class BattleGridDrawer
    {
        protected int dir;
        protected int originIndex;
        protected Vector2Int originPoint;
        protected List<Vector2Int> areaPoints;

        public BattleGridDrawer(int dir, int originIndex, int areaIndex)
        {
            this.dir = dir;
            this.originIndex = originIndex;
            this.originPoint = BattleUtils.Index2Coord(originIndex);
            this.areaPoints = new List<Vector2Int>();
        }

        public virtual void Draw(int index) { }
        public virtual void MoveUp(int index){ }
        public virtual void MoveDown(int index) { }
        public virtual void MoveLeft(int index){ }
        public virtual void MoveRight(int index){ }
    }

    public class SelectMovePointDrawer : BattleGridDrawer
    {
        private List<Vector2Int> ranagePoints;
        public SelectMovePointDrawer(int dir, int originIndex, int areaIndex) : base(dir, originIndex, areaIndex)
        {
            ranagePoints = BattleUtils.GetManhattanCircleAccessiblePoints(originPoint.x, originPoint.y, 3, (v) =>
            {
                var item = BattleUtils.GetTile(v.x, v.y);
                if (item != null)
                {
                    return item.Passable();
                }
                else
                {
                    return true;
                }
            });

            Game.GetSystem<BattleSystem>().GridManager.Draw(1, TileState.Blue, ranagePoints);
        }

        public override void Draw(int index)
        {
            Vector2Int coord = BattleUtils.Index2Coord(index);

            bool contains = BattleUtils.Contains(ranagePoints, coord);
            int state = contains ? TileState.Yellow : TileState.Red;

            List<Vector2Int> points = new List<Vector2Int>();
            points.Add(coord);
            Game.GetSystem<BattleSystem>().GridManager.Wipe(2);
            Game.GetSystem<BattleSystem>().GridManager.Draw(2, state, points);
        }
    }

    public class FixedPointEffectDrawer : BattleGridDrawer
    {
        private List<Vector2Int> ranagePoints;

        public FixedPointEffectDrawer(int dir, int originIndex, int areaIndex) : base(dir, originIndex, areaIndex)
        {
            ranagePoints = BattleUtils.GetManhattanCirclePoints(originPoint.x, originPoint.y, 4);

            Game.GetSystem<BattleSystem>().GridManager.Draw(1, TileState.Blue, ranagePoints);

            areaPoints = new List<Vector2Int>()
            {
                 new Vector2Int(0, 0),
                 new Vector2Int(0, 1),
                 new Vector2Int(1, 0),
                 new Vector2Int(-1, 0),
                 new Vector2Int(0, -1),
            };
        }

        public override void Draw(int index)
        {
            Vector2Int p = BattleUtils.Index2Coord(index);

            List<Vector2Int> points = new List<Vector2Int>();
            for (int i = 0; i < areaPoints.Count; i++)
            {
                var item = areaPoints[i];

                int x = p.x + item.x;
                int y = p.y + item.y;
                points.Add(new Vector2Int(x, y));
            }

            Game.GetSystem<BattleSystem>().GridManager.Wipe(2);
            Game.GetSystem<BattleSystem>().GridManager.Draw(2, TileState.Red, points);
        }
    }

    public class FixedDirectionEffectDrawer : BattleGridDrawer
    {
        private bool isDrawed;

        public FixedDirectionEffectDrawer(int dir, int originIndex, int areaIndex) : base(dir, originIndex, areaIndex)
        {
            isDrawed = false;

            areaPoints = new List<Vector2Int>()
            {
                 new Vector2Int(0, 0),
                 new Vector2Int(1, 0),
                 new Vector2Int(2, 0),
                 new Vector2Int(1, 1),
                 new Vector2Int(1, -1),
                 new Vector2Int(2, 2),
                 new Vector2Int(2, -2),
            };
        }

        public override void Draw(int index)
        {
            if (isDrawed)
            {
                return;
            }
            MoveRight(index);
            isDrawed = true;
        }

        public override void MoveUp(int index)
        {
            Vector2Int p = originPoint;

            List<Vector2Int> points = new List<Vector2Int>();
            for (int i = 0; i < areaPoints.Count; i++)
            {
                var item = areaPoints[i];

                int x = p.x + item.y * -1;
                int y = p.y + item.x;
                points.Add(new Vector2Int(x, y));
            }

            Game.GetSystem<BattleSystem>().GridManager.Wipe(1);
            Game.GetSystem<BattleSystem>().GridManager.Draw(1, TileState.Red, points);
        }

        public override void MoveDown(int index)
        {
            Vector2Int p = originPoint;
            List<Vector2Int> points = new List<Vector2Int>();

            for (int i = 0; i < areaPoints.Count; i++)
            {
                var item = areaPoints[i];

                int x = p.x + item.y;
                int y = p.y + item.x * -1;
                points.Add(new Vector2Int(x, y));
            }

            Game.GetSystem<BattleSystem>().GridManager.Wipe(1);
            Game.GetSystem<BattleSystem>().GridManager.Draw(1, TileState.Red, points);
        }

        public override void MoveLeft(int index)
        {
            Vector2Int p = originPoint;
            List<Vector2Int> points = new List<Vector2Int>();

            for (int i = 0; i < areaPoints.Count; i++)
            {
                var item = areaPoints[i];

                int x = p.x + item.x * -1;
                int y = p.y + item.y * -1;
                points.Add(new Vector2Int(x, y));
            }

            Game.GetSystem<BattleSystem>().GridManager.Wipe(1);
            Game.GetSystem<BattleSystem>().GridManager.Draw(1, TileState.Red, points);
        }

        public override void MoveRight(int index)
        {
            Vector2Int p = originPoint;
            List<Vector2Int> points = new List<Vector2Int>();

            for (int i = 0; i < areaPoints.Count; i++)
            {
                var item = areaPoints[i];
                int x = p.x + item.x * 1;
                int y = p.y + item.y * 1;
                points.Add(new Vector2Int(x, y));
            }
            Game.GetSystem<BattleSystem>().GridManager.Wipe(1);
            Game.GetSystem<BattleSystem>().GridManager.Draw(1, TileState.Red, points);
        }
    }

    [Gameplay]
    public class BattleViewSystem : IGameplaySystem
    {
        private BattleGrid m_BattleGrid;
        private BattleGridController m_Controller;

        private BattleGridDrawer drawer;

        public void OnInit()
        {

        }

        public void OnExit()
        {

        }

        public void OnEnterBattle()
        {
            GameRoot.GetNode<ActorNode>().ChangeBattleModel(true);

            var battleNode = GameRoot.GetNode<BattleNode>();
            m_BattleGrid = battleNode.LoadGrid<BattleGrid>(0, 0);

            m_Controller = new BattleGridController();
            m_Controller.Show(m_BattleGrid.NavigationView);
        }

        public void OnEixtBattle()
        {
            var battleNode = GameRoot.GetNode<BattleNode>();
            battleNode.DestroyGrid();

            GameRoot.GetNode<ActorNode>().ChangeBattleModel(false);

            m_BattleGrid = null;
            m_Controller = null;
        }

        public void FocusBattleGrid(int index, NavigateBattleGridType type)
        {
            drawer = null;
            if (type == NavigateBattleGridType.SelectMovePosition)
            {
                drawer = new SelectMovePointDrawer(1, index, 1);
            }

            if (type == NavigateBattleGridType.SelectEffectArea)
            {
                drawer = new FixedPointEffectDrawer(1, index, 1);
            }
           
            BattleGridCommand cammand = new BattleGridCommand(m_Controller, new int[] { index });
            Game.GetModule<InputController>().PushCammand(cammand);
        }

        public void SelectBattleTile(int index)
        {
            drawer?.Draw(index);
        }

        public void DeselectBattleTile(int index)
        {
            //Game.GetSystem<BattleSystem>().GridManager.Wipe();
        }

        public void SubmitBattleTile(int index)
        {
            DataModel param = new DataModel();
            param.SetValue(DataKey.Index, index);

            Game.GetSystem<BattleSystem>().FlowManager.MoveNext(param);
        }

        public void MoveUp(int index)
        {
            drawer?.MoveUp(index);
        }

        public void MoveDown(int index)
        {
            drawer?.MoveDown(index);
        }

        public void MoveLeft(int index)
        {
            drawer?.MoveLeft(index);
        }

        public void MoveRight(int index)
        {
            drawer?.MoveRight(index);
        }
    }
}
