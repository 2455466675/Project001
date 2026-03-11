namespace MVC
{
    public interface IBindingManager
    {
        Binder CreateBinder(int index);
        Binder CreateBinder(string key);
        void RemoveBinder(int index);
        void RemoveBinder(string key);
        void ClearBinding();
    }
}