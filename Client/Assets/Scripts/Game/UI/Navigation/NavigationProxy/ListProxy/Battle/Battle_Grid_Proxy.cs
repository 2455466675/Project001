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
            TileItem tileItem = item as TileItem;
            SceneGridView view = Parent.GetView<SceneGridView>();
            view.ResetAllItem();

            //List<(int, int)> path = view.AStarPath(10, 6, tileItem.X, tileItem.Y);
            //foreach (var p in path)
            //{
            //    TileItem t = view.GetTileItem(p.Item1, p.Item2);
            //    if (t != null) 
            //    {
            //        t.SetState(TitleState.Blue);
            //    }
            //}

            List<(int, int)> result = view.ReachableRange(tileItem.X, tileItem.Y, 5);
            foreach (var r in result)
            {
                TileItem t = view.GetTileItem(r.Item1, r.Item2);
                if (t != null)
                {
                    t.SetState(TitleState.Blue);
                }
            }
        }
    }
}
