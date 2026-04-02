using UnityEngine;

namespace GameFramework.Logic
{
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
