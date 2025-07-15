using Game.Event;

namespace Game.GSystem 
{
    public struct OnBattleGridSelectChangedEventArgs : IEventArgs
    {
        public TileItem tileItem;
    }

    public struct OnBattleGridSubmitEventArgs : IEventArgs
    {
        public TileItem tileItem;
    }

    [Event]
    public class OnBattleGridSubmitEventHandler : EventBase<OnBattleGridSubmitEventArgs>
    {
        public override void Invoke(OnBattleGridSubmitEventArgs arg)
        {
            var item = arg.tileItem;
            Game.System.BattleSystem.OnSubmitGrid(item.X, item.Y);
        }
    }
}
