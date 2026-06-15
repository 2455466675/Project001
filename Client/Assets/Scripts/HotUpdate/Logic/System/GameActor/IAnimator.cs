namespace GameFramework.Logic
{
    public interface IAnimator
    {
        public void SetAnimatorController(string controllerName);
        public void SetAnimatorValue(string name, bool value);
        public void SetAnimatorValue(string name, float value);
        public void SetAnimatorValue(string name, int value);
        public void SetAnimatorValue(string name);
        public void PlayAnimation(string name);
    }
}
