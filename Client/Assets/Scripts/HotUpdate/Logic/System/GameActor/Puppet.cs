using UnityEngine;

namespace GameFramework.Logic 
{
    public class Puppet : MonoBehaviour, IPuppet
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private ActorAnimator puppetAnimator;
        [SerializeField]
        private Transform[] bones;
        [SerializeField]
        private PuppetBone puppetBone;

        public float PlayAnimation(string name)
        {
            return puppetAnimator.PlayAnimation(name);
        }

        public void SetAnimatorController(string controllerName)
        {
            puppetAnimator.SetAnimatorController(controllerName);
        }

        public void SetAnimatorValue(string name, bool value)
        {
            puppetAnimator.SetAnimatorValue(name, value);
        }

        public void SetAnimatorValue(string name, float value)
        {
            puppetAnimator.SetAnimatorValue(name, value);
        }

        public void SetAnimatorValue(string name, int value)
        {
            puppetAnimator.SetAnimatorValue(name, value);
        }

        public void SetAnimatorValue(string name)
        {
            puppetAnimator.SetAnimatorValue(name);
        }

        public void AddWidget(PuppetWidgetArgs args)
        {
            puppetBone.AddWidget(args);
        }

        public void RemoveWidget(string key)
        {
            puppetBone.RemoveWidget(key);
        }
    }
}
