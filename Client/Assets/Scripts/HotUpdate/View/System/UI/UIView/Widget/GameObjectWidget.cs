using UnityEngine;

namespace GameFramework.View.UI
{
    public class GameObjectWidget : UIWidget
    {
        [SerializeField]
        private GameObject go;

        public T GetUEComponent<T>() where T : Component
        {
            if (go == null)
            {
                return default;
            }
            else
            {
                return go.GetComponent<T>();
            }
        }
    }
}