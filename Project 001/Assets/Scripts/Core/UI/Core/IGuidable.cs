using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGuidable
    {
        bool IsBeSelected { get; }
        Vector3 GuidePoint();
        void OnSubmit();
        void OnSelected();
        void OnDeselected();
        void OnMoveUp();
        void OnMoveDown();
        void OnMoveLeft();
        void OnMoveRight();
    }
}