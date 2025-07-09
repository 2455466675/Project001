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
            tileItem.SetState(true);
            Game.Event.Publish(new OnBattleGridSelectChangedEventArgs() { tileItem = tileItem });
        }

        public override void OnDeselect(GameNavigationItem item)
        {
            TileItem tileItem = item as TileItem;
            tileItem.SetState(false);
        }
    }
}
