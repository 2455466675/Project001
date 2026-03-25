namespace GameFramework.Core
{
    [GameMessage]
    public class GameStartMessage_Handler : GameMessageHandler<GameStartMessage>
    {
        public override void Receive(GameStartMessage message)
        {
            MDebug.Log("GameStartMessage_Handler");
            Game.GetSystem<GameStateSystem>().Start();
        }
    }
}