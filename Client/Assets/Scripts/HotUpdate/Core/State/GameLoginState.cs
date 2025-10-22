
namespace GameFramework.Core
{
    public class GameLoginState : StateItemBase
    {
        protected override void OnEnter()
        {
            Game.Event.Publish(new SwitchInputModuleEventArgs() { moduleType = InputModuleType.Character });
            Game.Event.Publish(new NavigateEventArgs() { navigationDefine = NavigationDefine.LoginList });
        }
    }
}