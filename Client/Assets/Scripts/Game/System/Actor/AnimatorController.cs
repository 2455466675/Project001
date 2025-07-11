using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GSystem
{
    [Serializable]
    public class ControllerItem
    {
        public AnimatorControllerType type;
        public RuntimeAnimatorController animator;
    }

    public class AnimatorController : MonoBehaviour
    {
        [SerializeField]
        private AnimatorControllerType defaultController;
        [SerializeField]
        private ControllerItem[] animators;

        public AnimatorControllerType DefaultController => defaultController;

        public RuntimeAnimatorController GetController(AnimatorControllerType type) 
        {
            if (animators == null || animators.Length == 0) 
            {
                return null;
            }

            ControllerItem item = Array.Find(animators, a => a.type == type);
            if (item != null) 
            {
                return item.animator;
            }
            else
            {
                return null;
            }
        }
    }
}