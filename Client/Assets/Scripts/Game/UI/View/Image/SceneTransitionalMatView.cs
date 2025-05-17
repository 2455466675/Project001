using UnityEngine;

namespace Game.UI 
{
    public class SceneTransitionalMatView : View
    {
        [SerializeField]
        private ExtendImage image;
        private Material materialInstance;

        private int _TransitionValueID;
        private void Awake()
        {
            if (image != null) 
            {
                materialInstance = new Material(image.material);
                image.material = materialInstance;

                _TransitionValueID = Shader.PropertyToID("_TransitionValue");
            }
        }

        public void SetValue(float value) 
        {
            if (materialInstance == null) 
            {
                return;
            }

            materialInstance.SetFloat(_TransitionValueID, value);
        }
    }
}