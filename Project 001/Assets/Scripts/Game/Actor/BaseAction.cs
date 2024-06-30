using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
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
        public void SetActor(Actor actor)
        {
            this.actor = actor;
        }
        public abstract void Execute();
        public abstract void Exit();
	}
}

