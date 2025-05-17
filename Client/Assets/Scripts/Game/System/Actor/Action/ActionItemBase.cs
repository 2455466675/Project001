using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GSystem
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ActionItemBase : MonoBehaviour
    {
        public int Priority => priority;
        public float Timepoint => timepoint;
        public float Duration => duration;

        [PropertySpace(SpaceBefore = 10, SpaceAfter = 0)]
        [SerializeField]
        [Min(0)]
        private int priority;
        [SerializeField]
        [Min(0f)]
        private float timepoint;
        [PropertySpace(SpaceBefore = 0, SpaceAfter = 20)]
        [SerializeField]
        private float duration;

        public abstract ActionCommandBase CreateCommand();
    }
}
