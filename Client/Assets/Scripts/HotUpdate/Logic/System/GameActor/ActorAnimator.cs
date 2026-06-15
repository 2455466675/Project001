using System;
using UnityEngine;

namespace GameFramework.Logic
{
    public class ActorAnimator : MonoBehaviour, IAnimator
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
        private Animator m_Animator;

        [SerializeField]
        [StringDropdown(Normal, Battle)]
        private string m_Default;

        [SerializeField]
        private AnimatorController[] m_Controllers;

        public void SetAnimatorController(string controllerName)
        {
            if (m_Animator == null)
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
                    m_Animator.runtimeAnimatorController = item.controller;
                    return;
                }
            }
        }

        public void SetAnimatorValue(string name, float value)
        {
            if (m_Animator != null)
            {
                m_Animator.SetFloat(name, value);
            }
        }

        public void SetAnimatorValue(string name, int value)
        {
            if (m_Animator != null)
            {
                m_Animator.SetInteger(name, value);
            }
        }

        public void SetAnimatorValue(string name, bool value)
        {
            if (m_Animator != null)
            {
                m_Animator.SetBool(name, value);
            }
        }

        public void SetAnimatorValue(string name)
        {
            if (m_Animator != null)
            {
                m_Animator.SetTrigger(name);
            }
        }

        public void PlayAnimation(string name)
        {
            if (m_Animator != null)
            {
                m_Animator.Play(name);
            }
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (m_Animator == null)
            {
                m_Animator = GetComponentInChildren<Animator>();
            }
        }
#endif
    }
}
