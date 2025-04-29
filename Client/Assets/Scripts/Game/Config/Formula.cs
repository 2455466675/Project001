using UnityEngine;

namespace Config
{
    /// <summary>
    /// 游戏数据定义
    /// </summary>
    [CreateAssetMenu(menuName= "MyMenu/Create Formula")]
	public class Formula : ScriptableObject
	{
        /// <summary>
        /// UI物体预制体路径
        /// </summary>
        public string UIRootPath;
        public string ActorContainerPath;
        /// <summary>
        /// ui导航配置
        /// </summary>
        public string NavigationMap;
        /// <summary>
        /// 场景配置
        /// </summary>
        public string SceneMap;
        /// <summary>
        /// 队伍人数上限
        /// </summary>
        public int PartyLimit;
        /// <summary>
        /// 角色队列跟随间隔
        /// </summary>
        public float PartyUnitGap;
	}
}

