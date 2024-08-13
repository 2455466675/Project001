using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGuidable
    {
        bool IsBeSelected { get; }
        GameObject CurrentGameObject { get; }
        RectTransform GuidePoint { get; }

        void OnSubmit();
        void OnSelected();
        void OnDeselected();
        void OnMoveUp();
        void OnMoveDown();
        void OnMoveLeft();
        void OnMoveRight();
    }
}