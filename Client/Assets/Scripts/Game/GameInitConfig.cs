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
        /// 游戏配置文件
        /// </summary>
        public string GameCfgFile;
        /// <summary>
        /// Formula路径
        /// </summary>
        public string Formula;
        /// <summary>
        /// 精灵配置
        /// </summary>
        public string SpriteMap;
    }
}
