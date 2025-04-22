namespace Navigation
{
    public interface INavigationItemData
    {
        public void Bind(IRefreshable obj);
        public void Unbind(IRefreshable obj);
    }
}