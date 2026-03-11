namespace GameFramework.View.UI
{
    public class NavigationItemView : UIWidgetContainer
    {
#if UNITY_EDITOR
        [Button("Init")]
        protected override void InitEditor()
        {
            base.InitEditor();
        }
#endif
    }
}