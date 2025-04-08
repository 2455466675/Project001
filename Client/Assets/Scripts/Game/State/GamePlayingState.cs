namespace Game.State
{
    /// <summary>
    /// 
    /// </summary>
    public class GamePlayingState : StateBase
    {
        public override GameStateDefine Define => GameStateDefine.Playing;

        public override void Enter()
        {            
            Game.UI.Close();
            Game.Scene.LoadSceneAsync(10003, null, null);
        }

        public override void Exit()
        {
            
        }
    }
}
