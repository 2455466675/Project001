using GameFramework.Core;

namespace GameFramework.UI
{
    [GameEvent]
    public class GameLoginEvent_Handler : GameEventHandlerBase<GameLoginEventArgs>
    {
        public override void Invoke(GameLoginEventArgs arg)
        {
            bool isEnter = arg.isEnter;
            UINode node = GameRoot.GetNode<UINode>();
            if (isEnter) 
            {
                node.ShowMask();
            }
            else
            {
                node.HideMask();
            }
        }
    }
}
