namespace Game
{
    public struct GameStartEventArg
    {

    }

    /// <summary>
    /// 
    /// </summary>
    [Event]
    public class GameStartEventHandler : GameEvent<GameStartEventArg>
    {
        public override void Run(GameStartEventArg arg)
        {
            GameWorld.Root.GetComponent<StateComponent>().Switch(GameStateDefine.Login);           
        }
    }
}
