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
        public IGuidable LastGuidable { get; private set; }
        public IGuidable CurrGuidable { get; private set; }

        public event Action<IGuidable> OnSelectGuidableChanged;

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

        public void SelectGuidable(IGuidable guidable)
        {
            LastGuidable?.OnDeselected();
            CurrGuidable = guidable;
            CurrGuidable?.OnSelected();
            OnSelectGuidableChanged?.Invoke(guidable);
        }

        public void Submit()
        {
            CurrGuidable?.OnSubmit();
        }

        public void InFocusNavigatable(INavigatable navigatable)
        {
            CurrNavigatable = navigatable;
        }
    }
}

