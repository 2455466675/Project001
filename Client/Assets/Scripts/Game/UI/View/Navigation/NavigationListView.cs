using Navigation;
using System.Collections.Generic;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class NavigationListView : View
    {        
        public bool IsValid => List != null;
        public abstract NavigationList List { get; }

        public NavigationListDefine define;

        public abstract void UpdateData(List<object> data);
    }
}
