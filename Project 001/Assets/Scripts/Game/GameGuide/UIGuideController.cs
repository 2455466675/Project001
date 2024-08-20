using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class UIGuideController : MonoBehaviour
    {
        public GameObject guideFinger;

        public void Awake()
        {
            guideFinger.SetActive(false);
            GameCore.UI.SetUIGuideController(this);
            GameCore.UI.OnSelectGuidableChanged += OnSelectGuidableChangedHandler;
        }

        public void OnDestroy()
        {
            GameCore.UI.OnSelectGuidableChanged -= OnSelectGuidableChangedHandler;
        }

        public void Show()
        {

        }

        public void Hide()
        {
            guideFinger.SetActive(false);
        }

        private void OnSelectGuidableChangedHandler(IGuidable guidableItem)
        {
            if (guidableItem == null)
            {
                guideFinger.SetActive(false);
                return;
            }

            guideFinger.SetActive(true);
            Camera camera = GameCore.UI.UICamera;
            Vector2 screenV2 = RectTransformUtility.WorldToScreenPoint(camera, guidableItem.GuidePoint());
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, screenV2, camera, out Vector2 targetV2);
            guideFinger.transform.localPosition = targetV2;
        }

    }
}