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
                    //有存档数据优先选中“读取游戏”，否则选中“开始游戏”
                    int index = Game.GetSystem<GameSaveSummary>().HasAnySaveData() ? 1 : 0;
                    Game.GetSystem<UISystem>().Navigate(NavigationDefine.LoginList, new int[] { index });
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
}