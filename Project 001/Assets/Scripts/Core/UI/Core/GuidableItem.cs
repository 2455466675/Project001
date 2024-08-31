

using Game.Core;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class GuidableItem<T> : GuidableItemBase where T : ItemDB
    {
        public T Dautm => GetItemDB<T>();

    }
}

