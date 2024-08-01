using UnityEngine;

namespace Game.Cfg
{
    /// <summary>
    /// 游戏数据定义
    /// </summary>
    [CreateAssetMenu(menuName= "MyMenu/Create Formula")]
	public class Formula : ScriptableObject
	{
        /// <summary>
        /// 队伍人数上限
        /// </summary>
        public int TEAM_LIMIT;

        /// <summary>
        /// 游戏系统配置
        /// </summary>
        public string GAME_SYSTEM_CONFIG_PATH;

        /// <summary>
        /// 角色队列跟随间隔
        /// </summary>
        public float TEAM_GAP;
	}
}

