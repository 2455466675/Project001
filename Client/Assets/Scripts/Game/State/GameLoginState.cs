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
           // GameWorld.Root.GetComponent<UIComponent>().Navigate(UI.NavigationListDefine.Login_List, UI.ModuleType.Panel);
            //GameWorld.Root.GetComponent<SceneComponent>().LoadScene(10001);
        }

        public override void Exit()
        {
            
        }
    }
}
