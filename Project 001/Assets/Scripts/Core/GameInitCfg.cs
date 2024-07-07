using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 游戏初始化配置
    /// </summary>
    [Serializable]
    public class GameInitCfg
    {
        /// <summary>
        /// 游戏配置文件
        /// </summary>
        public string GameCfgFile;
        /// <summary>
        /// UI物体预制体路径
        /// </summary>
        public string UIRootPath;
    }
}