using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class BattleActorLoader : MonoBehaviour
	{
        public Actor actor;

        [ReadOnly]
        [SerializeField]
        private string actorPath;

        public Actor LoadActor(string path)
        {
            actorPath = path;
            GameObject obj = GameCore.ResourceManager.LoadAndInstantiate(path, transform);
            actor = obj.GetComponent<Actor>();
            return actor;
        }
	}
}

