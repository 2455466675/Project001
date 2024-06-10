using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI
{
    public enum NavigationType 
    { 
        Horizontal,
        Vertical,
        Grid,
    }

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
    public class NavigationBox : MonoBehaviour
    {
        public NavigationType navigationType;

        public int defaultIndex;
        public int currIndex;
        public ListItem current;

        public void SelectDefault()
        {
            Select(defaultIndex);
        }

        public virtual void Select(int index)
        {         
        }

        public virtual void Submit()
        {

        }

        public virtual void MoveUp()
        {
        }

        public virtual void MoveDown() 
        { 
        }

        public virtual void MoveLeft()
        {
        }

        public virtual void MoveRight()
        {
        }
    }
}

