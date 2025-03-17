using Navigation;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationListView : MonoBehaviour
    {        
        public NavigationListDefine define;

        [SerializeField]
        private NavigationList list;
        public bool IsValid => list != null;

        public void Init()
        {
            if (list == null)
            {
                list = GetComponent<NavigationList>();
            }
        }

        private void OnValidate()
        {
            if (list == null) 
            {
                list = GetComponent<NavigationList>();
            }
        }
    }
}
