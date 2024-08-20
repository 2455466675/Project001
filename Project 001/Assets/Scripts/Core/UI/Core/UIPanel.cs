using Sirenix.OdinInspector;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class UIPanel : WindowComponent
    {
        public ListView[] views;
        public ListView DefaultView => views?[0];

        protected override void OnShow()
        {
            if (DefaultView != null)
            {
                GameCore.UI.SelectNavigatable(DefaultView);
            }
        }
        protected override void OnHide() 
        {
        
        }
        protected override void OnInFocus()
        {

        }

        protected override void OnOutFocus()
        {

        }

        [Button("Init")]
        private void Init()
        {
            window = GetComponentInParent<Window>();
            views = GetComponentsInChildren<ListView>();          
        }
    }
}

