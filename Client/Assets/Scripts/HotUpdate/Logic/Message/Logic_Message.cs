using GameFramework.Core;
using GameFramework.Utility.GameDefine;

namespace GameFramework.Logic
{
    public struct UIPanelMessage : IGameMessage
    {
        public bool isVisible;
        public PanelDefine panel;
    }
}
