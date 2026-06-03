using GameFramework.Core;
using GameFramework.Logic;
using GameFramework.View.UI;
using GameFramework.Utility.GameDefine;

namespace GameFramework.View
{
    public class ViewMessageHandler
    {
    }

    [GameMessage(-99)]
    public class GameStartMessage_Handler : GameMessageHandler<GameStartMessage>
    {
        public override void Receive(GameStartMessage message)
        {
            Game.Message.SendMessage(new UIPanelMessage() { isVisible = true, panel = Utility.GameDefine.PanelDefine.GameTransitionPanel });
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

    [GameMessage(99)]
    public class GamePlayMessage_Handler : GameMessageHandler<GamePlayMessage>
    {
        public override void Receive(GamePlayMessage message)
        {
            GamePlayStatus status = message.status;
            switch (status)
            {
                case GamePlayStatus.Begin:
                    Game.GetSystem<UISystem>().CloseNavigate();
                    break;
                case GamePlayStatus.Loaded:
                    break;
                case GamePlayStatus.Exit:
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

    [GameMessage]
    public class CreateEntity_Handler : GameMessageHandler<CreateEntityMessage>
    {
        public override void Receive(CreateEntityMessage message)
        {
            Game.GetSystem<GameProjector>().CreateProjection(message.eid);
        }
    }

    [GameMessage]
    public class ProjectComponent_Handler : GameMessageHandler<ProjectComponentMessage>
    {
        public override void Receive(ProjectComponentMessage message)
        {
            Game.GetSystem<GameProjector>().ProjectComponent(message.eid, message.type);
        }
    }
}