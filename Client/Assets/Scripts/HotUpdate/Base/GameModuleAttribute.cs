using System;

namespace GameFramework 
{
    /// <summary>
    /// 游戏模块特性类
    /// 通过Tools/MyTools/Game Priority Editor来编辑优先级
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GameModuleAttribute : GameAttribute
    {
        public GameModuleAttribute()
        {

        }
    }
}