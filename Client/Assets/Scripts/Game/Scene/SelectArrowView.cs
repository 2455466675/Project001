using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game 
{
    public class SelectArrowView : View
    {
        [SerializeField]
        private SpriteRenderer arrowSprite;

        public void SetPosition(Vector3 position) 
        {
            transform.position = new Vector3(position.x, 0.5f, position.z);
            transform.rotation = Quaternion.AngleAxis(70f, Vector3.right);
        }
    }
}


