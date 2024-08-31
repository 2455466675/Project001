using System;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class NavigationController
	{
        public INavigatable CurrNavigatable { get; private set; }
        public IGuidable[] LastGuidables { get; private set; }
        public IGuidable[] CurrGuidables { get; private set; }

        public event Action<IGuidable[]> OnSelectGuidableChanged;

        public void Move(Vector2 dir)
        {
            if (CurrNavigatable == null)
            {
                return;
            }

            if (dir.x > 0)
            {
                CurrNavigatable.MoveRight();
            }
            else if (dir.x < 0)
            {
                CurrNavigatable.MoveLeft();
            }

            if (dir.y > 0)
            {
                CurrNavigatable.MoveUp();
            }
            else if (dir.y < 0)
            {
                CurrNavigatable.MoveDown();
            }
        }

        public void SelectGuidable(params IGuidable[] guidables)
        {
            if (LastGuidables != null)
            {
                for (int i = 0; i < LastGuidables.Length; i++)
                {
                    LastGuidables[i]?.OnDeselected();
                }
            }
            LastGuidables = CurrGuidables;

            CurrGuidables = guidables;
            if (CurrGuidables != null)
            {
                for (int i = 0; i < CurrGuidables.Length; i++)
                {
                    CurrGuidables[i]?.OnSelected();
                }
            }

            OnSelectGuidableChanged?.Invoke(guidables);
        }

        public void Submit()
        {
            if (CurrGuidables != null)
            {
                for (int i = 0; i < CurrGuidables.Length; i++)
                {
                    CurrGuidables[i]?.OnSubmit();
                }
            }
        }

        public void InFocusNavigatable(INavigatable navigatable)
        {
            CurrNavigatable = navigatable;
        }
    }
}

