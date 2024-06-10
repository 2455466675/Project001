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
        RectTransform TargetTransform { get; }

        void OnSubmit();
        void OnSelect();
        void OnDeselect();
        void OnMoveToUp();
        void OnMoveToDown();
        void OnMoveToLeft();
        void OnMoveToRight();
    }
}