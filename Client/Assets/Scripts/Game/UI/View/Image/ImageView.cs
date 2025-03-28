using UnityEngine;

namespace Game.UI
{
    public class ImageView : View
    {
        [SerializeField]
        private ExtendImage image;

        public void SetSprite(string spriteName) 
        {
            if (image == null) 
            {
                return;
            }

            Sprite sprite = GameWorld.Root.GetComponent<ResourceComponent>().GetSprite(spriteName);
            if (sprite == null) 
            {
                MLog.Error($"{gameObject.name} : {spriteName} not find");
                return;
            }
            image.sprite = sprite;
        }

        private void OnValidate()
        {
            if (image == null) 
            {
                image = GetComponent<ExtendImage>();
            }
        }
    }
}