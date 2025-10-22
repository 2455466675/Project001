using UnityEngine;

namespace GameFramework.UI 
{
    public class ImageWidget : UIWidget
    {
        [SerializeField]
        private ExtendImage m_Image;

        public void SetSprite(string spriteName)
        {

        }

        public void SetSprite(Sprite sprite) 
        {
            if (m_Image != null) 
            {
                m_Image.sprite = sprite;
            }
        }

        private void OnValidate()
        {
            if (this.m_Image == null)
            {
                m_Image = GetComponent<ExtendImage>();
            }
        }
    }
}
