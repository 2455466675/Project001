namespace Game.UI
{
    public interface IViewContainer
    {
        public T GetView<T>() where T : View;
        public T GetView<T>(string viewName) where T : View;
        public void Init();
    }
}