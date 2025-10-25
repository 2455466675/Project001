using UnityEngine;

namespace GameFramework.UI 
{
    public abstract class UIView : MonoBehaviour
    {
        public void SetActive(bool active) 
        {
            this.gameObject.SetActive(active);
        }
    }
}