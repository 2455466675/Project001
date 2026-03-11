using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.View.UI 
{
    public class ImageWidget : UIWidget
    {
        [SerializeField]
        private Image image;

        public void SetSprite(string spriteName)
        {

        }

        public void SetSprite(Sprite sprite) 
        {
            if (image != null) 
            {
                image.sprite = sprite;
            }
        }

        private void OnValidate()
        {
            if (this.image == null)
            {
                image = GetComponent<Image>();
            }
        }
    }
}
