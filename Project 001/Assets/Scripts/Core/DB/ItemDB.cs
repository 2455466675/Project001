using System;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ItemDB : IComparable<ItemDB>
    {
        public abstract int CompareTo(ItemDB db);        

        public virtual bool Filter()
        {
            return true;
        }
    }
}

