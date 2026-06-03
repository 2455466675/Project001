using GameFramework.Core;
using GameFramework.Utility.GameDefine;
using System;

namespace GameFramework.Logic
{
    public struct CreateEntityMessage : IGameMessage
    {
        public int eid;
    }

    public struct ProjectComponentMessage : IGameMessage
    {
        public int eid;
        public Type type;
    }

    public struct DestroyEntityMessage : IGameMessage
    {
        public int eid;
    }

    public struct UIPanelMessage : IGameMessage
    {
        public bool isVisible;
        public PanelDefine panel;
    }

    public struct UINavigationMessage : IGameMessage
    {
        public int[] indexs;
        public NavigationDefine navigation;
    }
}
