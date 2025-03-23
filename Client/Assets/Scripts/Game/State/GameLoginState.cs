namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class GameLoginState : GameState
    {
        public override GameStateDefine Define => GameStateDefine.Login;

        public override void Enter()
        {
            GameWorld.Root.GetComponent<UIComponent>().Navigate(UI.NavigationListDefine.Test_List_1, UI.ModuleType.Panel);
            GameWorld.Root.GetComponent<SceneComponent>().LoadScene(10001);
        }

        public override void Exit()
        {
            
        }
    }
}
