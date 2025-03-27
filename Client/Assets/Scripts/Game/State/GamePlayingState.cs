namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class GamePlayingState : GameState
    {
        public override GameStateDefine Define => GameStateDefine.Playing;

        public override void Enter()
        {            
            GameWorld.Root.GetComponent<UIComponent>().Close();
            GameWorld.Root.GetComponent<SceneComponent>().LoadScene(10003);
        }

        public override void Exit()
        {
            
        }
    }
}
