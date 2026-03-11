namespace GameFramework.View.UI
{
    public abstract class UIView : MVC.View
    {
        public void SetActive(bool active) 
        {
            this.gameObject.SetActive(active);
        }
    }
}