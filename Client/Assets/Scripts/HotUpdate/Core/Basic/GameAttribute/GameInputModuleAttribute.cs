using System;

namespace GameFramework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GameInputModuleAttribute : GameAttribute
    {
        public GameInputModuleAttribute()
        {

        }
    }
}
