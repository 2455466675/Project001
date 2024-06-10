using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageView : View
    {
        public Image target;
        public string spriteName;

        public void SetSpriteByName(string spriteName)
        {
            this.spriteName = spriteName;
            UpdateSprite();
        }

        private void UpdateSprite()
        {
            if (target == null) 
            {
                return;
            }
            if (string.IsNullOrEmpty(spriteName))
            {
                return;
            }
            target.sprite = GameCore.ResourceManager.GetSprite(spriteName);
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            target = GetComponent<Image>();
        }
#endif
    }
}