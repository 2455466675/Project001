using Game.UI;
using UnityEngine;

namespace Game.GSystem 
{
    public class BattleUnitRootView : View
    {
        [SerializeField]
        private Canvas hudCanvas;

        public void SetCanvasCamera(Camera camera)
        {
            if (hudCanvas == null) 
            {
                return;
            }

            hudCanvas.renderMode = RenderMode.WorldSpace;
            hudCanvas.worldCamera = camera;
        }
    }
}

