using System;
using System.Collections;
using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class GameCoroutine : MonoBehaviour, ICore
    {
        public void Awake()
        {

        }

        public IEnumerator Init()
        {
            yield return null;
        }

        public void StartCo(IEnumerator routine) 
        { 
            StartCoroutine(routine);
        }

        public void StartCo(Func<object[], IEnumerator> func, object[] args)
        {
            StartCoroutine(func(args));       
        }

        public void WaitForSeconds(Action action, float seconds)
        {
            StartCoroutine(InnerCoroutine_1(action, seconds));
        }

        public void WaitForFrames(Action action, int frame = 1)
        {
            StartCoroutine(InnerCoroutine_2(action, frame));            
        }

        private IEnumerator InnerCoroutine_1(Action action, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            action?.Invoke();
        }

        private IEnumerator InnerCoroutine_2(Action action, int frame)
        {
            for (int i = 0; i <= Math.Max(0, frame); i++)
            {
                yield return null;

            }
            action?.Invoke();
        }
    }
}