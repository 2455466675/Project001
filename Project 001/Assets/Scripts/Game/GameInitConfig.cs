using System;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class GameInitConfig
    {
        /// <summary>
        /// 资源包名
        /// </summary>
        public string PackageName;
        /// <summary>
        /// Formula路径
        /// </summary>
        public string FormulaFilePath;
        /// <summary>
        /// 游戏配置文件
        /// </summary>
        public string GameCfgFile;
        /// <summary>
        /// UI物体预制体路径
        /// </summary>
        public string UIRootPath;
        /// <summary>
        /// ui导航配置
        /// </summary>
        public string NavigationConfigFilePath;
    }
}
