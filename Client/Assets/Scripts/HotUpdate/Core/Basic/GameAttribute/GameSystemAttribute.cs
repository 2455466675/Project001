using System;

namespace GameFramework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GameSystemAttribute : GameAttribute
    {
        public GameSystemAttribute()
        {

        }
    }
}