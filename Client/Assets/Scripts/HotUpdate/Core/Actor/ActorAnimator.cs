using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Core
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
        private AnimatorController[] controllers;

        public void SetAnimatorController(string name)
        {
            if (m_Animator == null)
            {
                return;
            }

            if (controllers == null || controllers.Length == 0)
            {
                return;
            }

            foreach (var item in controllers)
            {
                if (item.name == name)
                {
                    m_Animator.runtimeAnimatorController = item.controller;
                    return;
                }
            }
        }

        public void SetFloat(string name, float value)
        {
            if (m_Animator != null)
            {
                m_Animator.SetFloat(name, value);
            }
        }

        public void SetInteger(string name, int value)
        {
            if (m_Animator != null)
            {
                m_Animator.SetInteger(name, value);
            }
        }

        public void SetBool(string name, bool value)
        {
            if (m_Animator != null)
            {
                m_Animator.SetBool(name, value);
            }
        }

        public void SetTrigger(string name)
        {
            if (m_Animator != null)
            {
                m_Animator.SetTrigger(name);               
            }
        }

        public void PlayAnim(string name)
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
