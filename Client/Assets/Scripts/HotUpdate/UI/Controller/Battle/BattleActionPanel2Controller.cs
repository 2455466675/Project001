using UnityEngine;

namespace GameFramework.UI
{
    [UIPanelController(PanelDefine.BattleActionPanel2, NavigationDefine.BattleActionList2)]
    public class BattleActionPane2lController : PanelController
    {
        protected override void OnOutFocus()
        {
            //SetAlpha(0f);

            //var controller = Game.GetModule<UIManager>().GetPanelController(PanelDefine.BattleActionPanel);
            //controller?.SetAlpha(0f);
        }

        protected override void OnRefocus()
        {
            //SetAlpha(1f);

            //var controller = Game.GetModule<UIManager>().GetPanelController(PanelDefine.BattleActionPanel);
            //controller?.SetAlpha(1f);
        }
    }
}
