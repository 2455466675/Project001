using System;
using YooAsset;

namespace GameFrameworkAOT
{
    [Serializable]
    public class GameInitConfig
    {
        public string packageName = "DefaultPackage";

        public EPlayMode playMode;

        /// <summary>
        /// 游戏入口程序集
        /// </summary>
        public string entryAssemblyName = "HotUpdate_Base";

        /// <summary>
        /// 游戏入口类
        /// </summary>
        public string entryClassName = "GameFramework.Game";

        /// <summary>
        /// 游戏入口方法
        /// </summary>
        public string entryMethodName = "Start";
    }
}
