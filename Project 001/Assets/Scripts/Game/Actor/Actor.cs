using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
	public class Actor : MonoBehaviour
	{
        public SpriteRenderer sp;
        public Rigidbody2D rb;
        public Animator animator;
        public Bones bones;

        public ActionAsset[] actionAssets;

        public void Awake()
        {
            foreach (var ac in actionAssets)
            {
                ac.SetActor(this);
            }
        }

        public void PlayAction(string name)
        {
            foreach (var ac in actionAssets)
            {
                if (string.Equals(ac.actionName, name))
                {
                    ac.Execute();
                    return;
                }
            }
        }
    }
}

