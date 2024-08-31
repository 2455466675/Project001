using UnityEngine;

namespace Game.UI
{
    public enum MoveType
    {
        Up,
        Down,
        Left,
        Right,
    }

    public class IndexChangedEventArgs
    {
        public int MinIndex { get; private set; }
        public int MaxIndex { get; private set; }
        public GuidableItemBase[] Items { get; private set; }
        public IndexChangedEventArgs(int minIndex, int maxIndex, GuidableItemBase[] items)
        {
            MinIndex = minIndex;
            MaxIndex = maxIndex;
            Items = items;
        }
    }

    public class SelectChangedEventArgs
    {
        public bool IsSuccesss { get; private set; }
        public int[] Index { get; private set; }
        public GuidableItemBase[] Items { get; private set; }
        public SelectChangedEventArgs(bool isSuccess, int[] index, GuidableItemBase[] items)
        {
            IsSuccesss = isSuccess;
            Index = index;
            Items = items;
        }
    }

    /// <summary>
    /// 
    /// </summary>
	public abstract class GuidableBox : MonoBehaviour
	{
        public abstract int[] CurrIndex {get;}
        public abstract void Select(params int[] index);
        public abstract void Move(MoveType moveType);        
	}
}

