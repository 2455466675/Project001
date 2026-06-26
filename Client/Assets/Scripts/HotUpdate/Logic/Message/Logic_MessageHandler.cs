using GameFramework.Core;
using GameFramework.Utility.GameDefine;

namespace GameFramework.Logic
{
    public class LogicMessageHandler
    {

    }

    [GameMessage]
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
                    Game.GetSystem<GameCameraController>().SetCameraModel(CameraModel.Follow);
                    Game.GetModule<PartyModule>().StartUp();
                    break;
                case GamePlayStatus.Exit:
                    break;
            }
        }
    }

    [GameMessage(98)]
    public class GameStartMessage_Handler : GameMessageHandler<GameStartMessage>
    {
        public override void Receive(GameStartMessage message)
        {
            SaveTypeRegistration.Register();
            Game.GetSystem<GameCameraController>().Start();
        }
    }
}