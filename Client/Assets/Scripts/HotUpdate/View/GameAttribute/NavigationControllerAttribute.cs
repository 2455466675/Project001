using System;
using GameFramework.Utility.GameDefine;

namespace GameFramework.View 
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class NavigationControllerAttribute : GameAttribute
    {
        public NavigationDefine Id { get; private set; }

        public NavigationControllerAttribute(NavigationDefine id) 
        { 
            Id = id;
        }
    }
}