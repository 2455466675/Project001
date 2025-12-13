using UnityEngine;

namespace GameFramework.Core
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

    public interface IActor : IAnimator
    {
        public Transform Transform { get; }
        public Vector3 Velocity { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 LocalPosition { get; set; }
        public Quaternion Rotation { get; set; }
        public void MovePosition(Vector3 pos);
        public void SetKinematic(bool isKinematic);
        Transform GetBone(string name);
    }
}
