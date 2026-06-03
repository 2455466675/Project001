using GameFramework.Utility.GameDefine;

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
        public GamePlayStatus status;
    }

    public struct GameTransitionFadeInMessage : IGameMessage
    {
        public TransitionType transitionType;
        public float fadeInTime;
    }

    public struct GameTransitionFadeOutMessage : IGameMessage
    {
        public TransitionType transitionType;
        public float fadeOutTime;
    }

    public struct GameTransitionProgressMessage : IGameMessage
    {
        public TransitionType transitionType;
        public float progress;
    }
}
