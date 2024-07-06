using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
	public static class UnityExtend
	{
        /// <summary>
        /// ≤È’“Parameter
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static AnimatorControllerParameter FindParameter(this Animator animator, string name)
        {
            foreach (var item in animator.parameters)
            {
                if (item.name == name)
                    return item;
            }
            return null;
        }
    }
}

