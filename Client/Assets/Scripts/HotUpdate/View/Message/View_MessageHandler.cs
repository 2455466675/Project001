using GameFramework.Core;
using GameFramework.View.UI;
using GameFramework.Utility.GameDefine;
using GameFramework.Logic;

namespace GameFramework.View
{
    public class ViewMessageHandler
    {
    }

    [GameMessage(99)]
    public class GameStartMessage_Handler : GameMessageHandler<GameStartMessage>
    {
        public override void Receive(GameStartMessage message)
        {
        }
    }

    [GameMessage]
    public class GameLoginMessage_Handler : GameMessageHandler<GameLoginMessage>
    {
        public override void Receive(GameLoginMessage message)
        {
            int status = message.status;
            switch (status)
            {
                case 0:
                    GameRoot.GetNode<UINode>().ShowMask();
                    break;
                case 1:
                    Game.GetSystem<UISystem>().Navigate(NavigationDefine.LoginList);
                    break;
                case 2:
                    GameRoot.GetNode<UINode>().HideMask();
                    break;                
            }            
        }
    }

    [GameMessage]
    public class GamePlayMessage_Handler : GameMessageHandler<GamePlayMessage>
    {
        public override void Receive(GamePlayMessage message)
        {
            int status = message.status;
            switch (status)
            {
                case 0:
                    Game.GetSystem<UISystem>().CloseNavigate();
                    Game.GetSystem<UISystem>().ShowPanel(PanelDefine.LoadingPanel);
                    break;
                case 1:
                    Game.GetSystem<UISystem>().HidePanel(PanelDefine.LoadingPanel);
                    break;
                case 2:

                    break;
            }
        }
    }

    [GameMessage]
    public class UIPanelMessage_Handler : GameMessageHandler<UIPanelMessage>
    {
        public override void Receive(UIPanelMessage message)
        {
            if (message.isVisible)
            {
                Game.GetSystem<UISystem>().ShowPanel(message.panel);
            }
            else
            {
                Game.GetSystem<UISystem>().HidePanel(message.panel);
            }
        }
    }

    [GameMessage]
    public class UINavigationMessage_Handler : GameMessageHandler<UINavigationMessage>
    {
        public override void Receive(UINavigationMessage message)
        {            
            Game.GetSystem<UISystem>().Navigate(message.navigation, message.indexs);
        }
    }
}