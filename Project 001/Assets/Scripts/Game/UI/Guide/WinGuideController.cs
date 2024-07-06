using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class WinGuideController : MonoBehaviour
    {
        public GameObject guideFinger;

        public void Awake()
        {
            GameCore.UI.AddSelectUIEventListener(OnSelectItem);
            GameCore.UI.AddDeselectUIEventListener(OnDeselectItem);
        }

        public void OnDestroy()
        {
            GameCore.UI.RemoveSelectUIEventListener(OnSelectItem);
            GameCore.UI.RemoveDeselectUIEventListener(OnDeselectItem);
        }

        private void OnSelectItem(IGuidable guidableItem)
        {
            guideFinger.SetActive(true);
            Camera camera = GameCore.UI.UICamera;
            Vector2 screenV2 = RectTransformUtility.WorldToScreenPoint(camera, guidableItem.TargetTransform.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, screenV2, camera, out Vector2 targetV2);
            guideFinger.transform.localPosition = targetV2;
        }

        private void OnDeselectItem(IGuidable guidableItem)
        {
            guideFinger.SetActive(false);
        }
    }
}