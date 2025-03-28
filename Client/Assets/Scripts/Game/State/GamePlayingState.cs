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
            GameWorld.Root.GetComponent<SceneComponent>().LoadSceneAsync(10003, null, null);
        }

        public override void Exit()
        {
            
        }
    }
}
