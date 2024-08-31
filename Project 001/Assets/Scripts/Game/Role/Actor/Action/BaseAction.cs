using UnityEngine;

namespace Game.System
{
    public struct ActionArgs 
    {
        public int iV;
        public float fV;
        public bool bV;
        public string sV;
        public Vector2 v2V;
    }

    /// <summary>
    /// 
    /// </summary>
	public abstract class BaseAction : MonoBehaviour
	{
        protected Actor actor;
        public abstract void Execute(Actor actor);
        public abstract void Exit();
	}
}

