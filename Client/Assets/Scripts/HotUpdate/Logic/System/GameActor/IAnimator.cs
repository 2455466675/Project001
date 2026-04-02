namespace GameFramework.Logic
{
    public interface IAnimator
    {
        public void SetAnimatorController(string name);
        public void SetBool(string name, bool value);
        public void SetFloat(string name, float value);
        public void SetInteger(string name, int value);
        public void SetTrigger(string name);
        public void PlayAnim(string name);
    }
}
