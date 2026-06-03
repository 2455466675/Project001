using UnityEngine;

namespace GameFramework.View.UI
{
    public class UITransitionWidget : UIWidget
    {
        [SerializeField]
        protected UITransitionWidgetContainer container;

        public virtual void FadeIn(float time)
        {
        }

        public virtual void FadeOut(float time) 
        {        
        }

        public virtual void Transition(float progress)
        {

        }
    }
}