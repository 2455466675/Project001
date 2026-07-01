using GameFramework.Utility.GameDefine;

namespace GameFramework.View.UI 
{
    [UIPanelController(PanelDefine.BattleFormation, NavigationDefine.BattlePlayer, NavigationDefine.BattleEnemy)]
    public class BattleFormationController : PanelController
    {
        protected override string AssetPath => "Assets/Bundles/UI/Prefabs/Panel/Battle/BattleFormation";
        protected override GroupType PanelGroup => GroupType.Battle;

        protected override void OnShow()
        {            
            MDebug.Log("加载战斗阵型");
        }

        protected override void OnHide()
        {            
            MDebug.Log("卸载战斗阵型");
        }
    }
}