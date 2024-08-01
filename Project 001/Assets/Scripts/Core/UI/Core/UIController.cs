using Game.Core;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class UIController : MonoBehaviour
    {
        public Window window;

        public ListView defaultListView;

        protected virtual void Awake()
        {
            window = GetComponentInParent<Window>();
            if (window != null)
            {
                window.SetController(this);
            }
        }

        private void Start()
        {
            Register();
        }

        public void OnShow()
        {

        }
       
        public void OnEnter()
        {
            if (defaultListView == null) 
            {
                return;
            }
            GameCore.UI.SelectListView(defaultListView, window.Id);
        }

        /// <summary>
        /// ‘⁄’‚¿Ô◊¢≤·DB
        /// </summary>
        protected virtual void Register()
        {

        }
    }
}