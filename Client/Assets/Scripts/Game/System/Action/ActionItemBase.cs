using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ActionItemBase : MonoBehaviour
    {
        public int Priority => Priority;
        public float Timepoint => timepoint;
        public float Duration => duration;

        [SerializeField]
        [Min(0)]
        private int priority;
        [SerializeField]
        [Min(0f)]
        private float timepoint;
        [SerializeField]
        private float duration;

        public abstract ActionCommandBase CreateCommand();
    }
}
