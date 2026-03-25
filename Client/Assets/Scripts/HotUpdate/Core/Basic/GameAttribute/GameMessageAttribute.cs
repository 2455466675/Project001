using System;

namespace GameFramework.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GameMessageAttribute : GameAttribute
    {
        public int priority;

        public GameMessageAttribute()
        {
        }

        public GameMessageAttribute(int priority)
        {
            this.priority = priority;
        }
    }
}