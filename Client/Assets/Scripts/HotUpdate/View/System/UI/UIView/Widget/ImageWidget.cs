using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.View.UI 
{
    public class ImageWidget : UIWidget
    {
        [SerializeField]
        private Image image;

        private Sprite lastSprite;

        public void SetSprite(string spriteName)
        {
            if (image == null)
            {
                return;
            }

            ReleaseSprite();
            Sprite sprite = Game.Assets.GetSprite(spriteName);
            if (sprite != null)
            {
                image.sprite = sprite;
                lastSprite = sprite;
            }
        }

        private void OnDestroy()
        {
            ReleaseSprite();
        }

        private void ReleaseSprite()
        {
            if (lastSprite != null)
            {
                Game.Assets.ReleaseAsset(lastSprite);
                lastSprite = null;
            }
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (this.image == null)
            {
                image = GetComponent<Image>();
            }
        }

#endif

    }
}
