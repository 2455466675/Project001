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

    /// <summary>
    /// 
    /// </summary>
	public abstract class GuidableBox : MonoBehaviour
	{
        public abstract int CurrIndex {get;}
        public abstract void Select(int index);
        public abstract void Move(MoveType moveType);        
	}
}

