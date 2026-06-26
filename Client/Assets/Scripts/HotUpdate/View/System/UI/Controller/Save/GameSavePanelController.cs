using UnityEngine;

namespace GameFramework.View.UI
{
    [UIPanelController(Utility.GameDefine.PanelDefine.GameSavePanel, Utility.GameDefine.NavigationDefine.GameSaveList)]
    public class GameSavePanelController : PanelController
    {
        protected override string AssetPath => "Assets/Bundles/UI/Prefabs/Panel/Save/GameSavePanel";
        protected override GroupType PanelGroup => GroupType.Normal;
    }
}
