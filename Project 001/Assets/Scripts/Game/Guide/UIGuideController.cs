using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class UIGuideController : MonoBehaviour
    {
        public GameObject guideFinger;

        private List<GameObject> fingers;

        public void Awake()
        {
            guideFinger.SetActive(false);
            fingers = new List<GameObject>();
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
            HideFingers();
        }

        private void OnSelectGuidableChangedHandler(IGuidable[] guidables)
        {
            HideFingers();
            if (guidables == null || guidables.Length == 0)
            {
                return;
            }

            for (int i = 0; i < guidables.Length; i++)
            {
                GameObject finger;
                if (i >= fingers.Count)
                {
                    finger = GoHelper.Instantiate(guideFinger, transform);
                    fingers.Add(finger);
                }
                else
                {
                    finger = fingers[i];
                }

                IGuidable guidableItem = guidables[i];
                Camera camera = GameCore.UI.UICamera;
                Vector2 screenV2 = RectTransformUtility.WorldToScreenPoint(camera, guidableItem.GuidePoint());
                RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, screenV2, camera, out Vector2 targetV2);

                finger.SetActive(true);
                finger.transform.localPosition = targetV2;
            }  
        }

        private void HideFingers()
        {
            for (int i = 0; i < fingers.Count; i++)
            {
                fingers[i].SetActive(false);
            }
        }
    }
}