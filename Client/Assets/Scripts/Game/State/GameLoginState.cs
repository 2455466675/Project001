namespace Game.State
{
    /// <summary>
    /// 
    /// </summary>
    public class GameLoginState : StateBase
    {
        public override GameStateDefine Define => GameStateDefine.Login;

        public override void Enter()
        {
            //GameWorld.Root.GetComponent<UIComponent>().Navigate(UI.NavigationListDefine.Test_List_1, UI.ModuleType.Panel);
           Game.UI.Navigate(UI.NavigationListDefine.Login_List, UI.Input.ModuleType.Panel);
           Game.Scene.LoadScene(10001);
        }

        public override void Exit()
        {
            
        }
    }
}
