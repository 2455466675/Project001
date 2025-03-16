namespace Game.UI
{
    public enum ModuleType 
    {
        Basal  = 1,
        Panel  = 2,
        Battle = 3,
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class InputModule : InputCammand
    {
        public abstract ModuleType ModuleType { get;}

        public virtual void Navigate(NavigationListDefine list_ID) { }
    }
}
