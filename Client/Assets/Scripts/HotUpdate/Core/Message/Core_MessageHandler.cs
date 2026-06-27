using GameFramework.Utility.GameDefine;

namespace GameFramework.Core
{
    [GameMessage(-99)]
    public class GamePlayMessage_Handler : GameMessageHandler<GamePlayMessage>
    {
        public override void Receive(GamePlayMessage message)
        {
        }
    }

    [GameMessage]
    public class GameStartMessage_Handler : GameMessageHandler<GameStartMessage>
    {
        public override void Receive(GameStartMessage message)
        {
            Game.GetSystem<GameStateSystem>().Start();
        }
    }
}