using UnityEngine;

namespace Navigation
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationGroup : MonoBehaviour
    {
        public void Init()
        {
        }

        public void Enter(bool isRefocus) 
        {
            OnEnter(isRefocus);
        }

        public void Exit() 
        {        
            OnExit();
        }

        public void OutFocus() 
        {        
            OnOutFocus();
        }

        protected virtual void OnEnter(bool isRefocus) 
        {
        }

        protected virtual void OnExit() 
        {
        }

        protected virtual void OnOutFocus() 
        {
        }
    }
}
