namespace GameFramework.Core
{
    public struct GameStartMessage : IGameMessage
    {
    }

    public struct GameLoginMessage : IGameMessage
    {
        public int status;
    }

    public struct GamePlayMessage : IGameMessage
    {
        public int status;
    }
}
