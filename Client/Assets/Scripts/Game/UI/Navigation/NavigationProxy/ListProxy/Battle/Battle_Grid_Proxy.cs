using Game.Event;
using Game.GSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public struct OnBattleGridSelectChangedEventArgs : IEventArgs
    {
        public TileItem tileItem;
    }

    /// <summary>
    /// 
    /// </summary>
    [ListProxy(NavigationListDefine.Battle_Grid)]
    public class Battle_Grid_Proxy : NavigationListProxy
    {
        public override void OnEnable()
        {
            MLog.Log("Battle_Grid_Proxy OnEnable");
        }

        public override void OnDisable()
        {
            MLog.Log("Battle_Grid_Proxy OnDisable");
        }

        public override void OnBindData(GameNavigationItem item)
        {

        }

        public override void OnUnbindData(GameNavigationItem item)
        {

        }

        public override void OnSelect(GameNavigationItem item)
        {
            MLog.Log("Battle_Grid_Proxy OnSelect");
            TileItem tileItem = item as TileItem;
            tileItem.Focus(true);
            Game.Event.Publish(new OnBattleGridSelectChangedEventArgs() { tileItem = tileItem });
        }

        public override void OnDeselect(GameNavigationItem item)
        {
            TileItem tileItem = item as TileItem;
            tileItem.Focus(false);
        }

        public override void OnSubmit(GameNavigationItem item)
        {
            BattleUnit unit = Game.System.BattleSystem.GetBattleUnit(0);
            TileItem tileItem = item as TileItem;
            SceneGridView view = Parent.GetView<SceneGridView>();
            view.ResetAllItem();

            var btc = unit.GetComponent<BattleTransformComponent>();

            List<Vector2Int> path = view.AStarPath(btc.pX, btc.pY, tileItem.X, tileItem.Y);

            if (path.Count < 2) 
            {
                return;
            }

            foreach (var p in path)
            {
                TileItem t = view.GetTileItem(p.x, p.y);
                if (t != null)
                {
                    t.SetState(TitleState.Blue);
                }
            }

            List<Vector2Int> temp = new List<Vector2Int>();
            var p0 = path[0];
            var p1 = path[1];
            bool isH = p0.y == p1.y;

            temp.Add(p0);

            var ep = p1;

            if (path.Count == 2) 
            {
                temp.Add(p1);
            }
            else
            {                
                for (int i = 2; i < path.Count; i++)
                {
                    var p = path[i];

                    if (isH)
                    {
                        if (p.y != ep.y)
                        {
                            temp.Add(path[i - 1]);
                            ep = p;
                            isH = false;
                        }
                    }
                    else
                    {
                        if (p.x != ep.x)
                        {
                            temp.Add(path[i - 1]);
                            ep = p;
                            isH = true;
                        }
                    }

                    if (i == path.Count - 1)
                    {
                        temp.Add(p);
                    }
                }
            }

            btc.MoveByPath(temp);

            //List<(int, int)> result = view.ReachableRange(tileItem.X, tileItem.Y, 5);
            //foreach (var r in result)
            //{
            //    TileItem t = view.GetTileItem(r.Item1, r.Item2);
            //    if (t != null)
            //    {
            //        t.SetState(TitleState.Blue);
            //    }
            //}
        }
    }
}
