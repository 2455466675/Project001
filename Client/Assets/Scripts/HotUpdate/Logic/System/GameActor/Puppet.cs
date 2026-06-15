using UnityEngine;

namespace GameFramework.Logic 
{
    public class Puppet : MonoBehaviour, IAnimator
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private ActorAnimator puppetAnimator;
        [SerializeField]
        private Transform[] bones;

        public void PlayAnimation(string name)
        {
            ((IAnimator)puppetAnimator).PlayAnimation(name);
        }

        public void SetAnimatorController(string controllerName)
        {
            ((IAnimator)puppetAnimator).SetAnimatorController(controllerName);
        }

        public void SetAnimatorValue(string name, bool value)
        {
            ((IAnimator)puppetAnimator).SetAnimatorValue(name, value);
        }

        public void SetAnimatorValue(string name, float value)
        {
            ((IAnimator)puppetAnimator).SetAnimatorValue(name, value);
        }

        public void SetAnimatorValue(string name, int value)
        {
            ((IAnimator)puppetAnimator).SetAnimatorValue(name, value);
        }

        public void SetAnimatorValue(string name)
        {
            ((IAnimator)puppetAnimator).SetAnimatorValue(name);
        }

        public Transform GetBone(string name)
        {
            if (bones == null || bones.Length == 0)
            {
                return null;
            }

            foreach (var item in bones)
            {
                if (item.gameObject.name == name)
                {
                    return item;
                }
            }

            return bones[0];
        }
    }
}