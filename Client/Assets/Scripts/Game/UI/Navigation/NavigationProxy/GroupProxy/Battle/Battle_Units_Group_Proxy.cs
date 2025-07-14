using Game.GSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    [GroupProxy(NavigationGroupDefine.Battle_Units_Group)]
    public class Battle_Units_Group_Proxy : NavigationGroupProxy
    {
        public override void LoadGroup(NavigationGroupDefine define)
        {
            GameObject go = GameObject.FindGameObjectWithTag("BattleUnits");
            if (go == null)
            {
                MLog.Error("BattleUnits is null");
                return;
            }
            MLog.Log("Battle_Units_Group_Proxy LoadGroup");
            groupView = go.GetComponent<NavigationGroupView>();
        }

        public override void Show()
        {
            if (groupView == null)
            {
                return;
            }
            groupView.gameObject.SetActive(true);

            Game.Event.Register<OnBattleGridSelectChangedEventArgs>(OnBattleGridSelectChanged);
            Game.System.BattleSystem.SetProxy(this);

            BattleUnitRootView view = GetView<BattleUnitRootView>();
            view.SetCanvasCamera(Game.Root.MainCamera.GameMainCamera);
        }

        public override void Hide()
        {
            if (groupView == null)
            {
                return;
            }
            groupView.gameObject.SetActive(false);
            Game.System.BattleSystem.SetProxy(null);

            BattleUnitRootView view = GetView<BattleUnitRootView>();
            view.SetCanvasCamera(null);
        }

        private void OnBattleGridSelectChanged(OnBattleGridSelectChangedEventArgs args) 
        {
            TileItem item  = args.tileItem;
            Vector3 pos = item.transform.position;
            SelectArrowView view = GetView<SelectArrowView>();
            view.SetPosition(pos);

            Game.Root.MainCamera.SetPosition(new Vector3(6, 6, 0));
            Game.Root.MainCamera.SetRotation(Quaternion.AngleAxis(70f, Vector3.right));
        }
    }
}
