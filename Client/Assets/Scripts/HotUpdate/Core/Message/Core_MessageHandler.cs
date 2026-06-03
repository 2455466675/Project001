using GameFramework.Utility.GameDefine;

namespace GameFramework.Core
{
    [GameMessage(-99)]
    public class GamePlayMessage_Handler : GameMessageHandler<GamePlayMessage>
    {
        public override void Receive(GamePlayMessage message)
        {
            GamePlayStatus status = message.status;
            switch (status)
            {
                case GamePlayStatus.Begin:

                    break;
                case GamePlayStatus.Loaded:
                    Game.GetSystem<GameSaveSystem>().LoadGame(0);
                    Game.GetSystem<GameInputSystem>().Switch(InputModuleType.Normal);                    
                    break;
                case GamePlayStatus.Exit:
                    break;
            }
        }
    }

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