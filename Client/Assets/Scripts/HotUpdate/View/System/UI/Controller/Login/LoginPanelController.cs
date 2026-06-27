using GameFramework.Utility.GameDefine;

namespace GameFramework.View.UI
{
    [UIPanelController(PanelDefine.LoginPanel, NavigationDefine.LoginList)]
    public class LoginPanelController : PanelController
    {
        protected override string AssetPath => "Assets/Bundles/UI/Prefabs/Panel/LoginPanel";
    }
}