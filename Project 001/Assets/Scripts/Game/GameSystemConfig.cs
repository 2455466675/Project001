using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(menuName= "MyMenu/Create GameSystemConfig")]
	public class GameSystemConfig : ScriptableObject
    {
        [SerializeField]
        private ActionContainer actionContainer;

        public string[] bones;

        public ActionGroup FindAction(string actionName) 
        {
            return actionContainer.GetAction(actionName);
        }
    }
}

