using System;

namespace GameFramework.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GameMessageAttribute : GameAttribute
    {
        public GameMessageAttribute()
        {

        }
    }
}