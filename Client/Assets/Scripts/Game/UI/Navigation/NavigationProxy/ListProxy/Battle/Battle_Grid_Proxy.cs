using Game.GSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
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

        public override bool InFocus(bool isRefocus, int[] indexs = null)
        {
            MLog.Log("Battle_Grid_Proxy InFocus");
            return base.InFocus(isRefocus, indexs);
        }

        public override void OutFocus()
        {
            MLog.Log("Battle_Grid_Proxy OutFocus");
            base.OutFocus();
        }

        public override void Exit()
        {
            MLog.Log("Battle_Grid_Proxy Exit");
            base.Exit();
        }
      
        public override void OnBindData(GameNavigationItem item)
        {

        }

        public override void OnUnbindData(GameNavigationItem item)
        {

        }

        public override void Move(float h, float v)
        {
            base.Move(h, v);
        }

        public override void Submit()
        {
            base.Submit();
        }

        public override void OnSelect(GameNavigationItem item)
        {
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
            Game.Event.Publish(new OnBattleGridSubmitEventArgs() { tileItem = tileItem });
            Game.UI.Back();
        }
    }
}
