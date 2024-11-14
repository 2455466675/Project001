using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(menuName= "MyMenu/Create GameSystemConfig")]
	public class GameSystemConfig : ScriptableObject
    {
        public ActionAssets commonActionAssets;

        public string[] bones;
    }
}

