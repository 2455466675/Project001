namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    [GroupProxy(NavigationGroupDefine.Battle_Loading_Group)]
    public class Battle_Loading_Group_Proxy : NavigationGroupProxy
    {
        public override void Show()
        {
            base.Show();
            Game.Event.Register<SceneLoadingProgress>(OnProgressUpdate);
        }

        public override void Hide()
        {
            base.Hide();
            Game.Event.Unregister<SceneLoadingProgress>(OnProgressUpdate);
        }

        private void OnProgressUpdate(SceneLoadingProgress arg)
        {
            var view = groupView.GetView<SceneTransitionalMatView>();
            if (view == null)
            {
                return;
            }
            view.SetValue(arg.progress);
        }
    }
}
