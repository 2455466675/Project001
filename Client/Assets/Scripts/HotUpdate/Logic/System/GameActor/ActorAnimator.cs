using System;
using UnityEngine;

namespace GameFramework.Logic
{
    public class ActorAnimator : MonoBehaviour
    {
        public const string Normal = "normal";
        public const string Battle = "battle";

        [Serializable]
        private class AnimatorController
        {
            [StringDropdown(Normal, Battle)]
            public string name;

            public RuntimeAnimatorController controller;
        }

        [SerializeField]
        private Animator mAnimator;

        [SerializeField]
        [StringDropdown(Normal, Battle)]
        private string m_Default;

        [SerializeField]
        private AnimatorController[] m_Controllers;

        public void SetAnimatorController(string controllerName)
        {
            if (mAnimator == null)
            {
                return;
            }

            if (m_Controllers == null || m_Controllers.Length == 0)
            {
                return;
            }

            foreach (var item in m_Controllers)
            {
                if (item.name == controllerName)
                {
                    mAnimator.runtimeAnimatorController = item.controller;
                    return;
                }
            }
        }

        public void SetAnimatorValue(string name, float value)
        {
            if (mAnimator != null)
            {
                mAnimator.SetFloat(name, value);
            }
        }

        public void SetAnimatorValue(string name, int value)
        {
            if (mAnimator != null)
            {
                mAnimator.SetInteger(name, value);
            }
        }

        public void SetAnimatorValue(string name, bool value)
        {
            if (mAnimator != null)
            {
                mAnimator.SetBool(name, value);
            }
        }

        public void SetAnimatorValue(string name)
        {
            if (mAnimator != null)
            {
                mAnimator.SetTrigger(name);
            }
        }

        public float PlayAnimation(string name)
        {
            float animLength = 0f;
            if (mAnimator != null)
            {
                AnimationClip[] clips = mAnimator.runtimeAnimatorController.animationClips;
                foreach (AnimationClip clip in clips)
                {
                    if (clip != null && clip.name == name)
                    {

                        animLength = clip.length;
                        break;
                    }
                }
                mAnimator.Play(name);
            }
            return animLength;
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (mAnimator == null)
            {
                mAnimator = GetComponentInChildren<Animator>();
            }
        }
#endif
    }
}
