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
            Game.Event.Register<SceneLoadingProgressEventArgs>(OnProgressUpdate);
        }

        public override void Hide()
        {
            base.Hide();
            Game.Event.Unregister<SceneLoadingProgressEventArgs>(OnProgressUpdate);
        }

        private void OnProgressUpdate(SceneLoadingProgressEventArgs arg)
        {
            var view = GetView<SceneTransitionalMatView>();
            if (view == null)
            {
                return;
            }
            view.SetValue(arg.progress);
        }
    }
}
