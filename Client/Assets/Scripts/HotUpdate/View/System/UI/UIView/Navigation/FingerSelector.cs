using Navigation;
using UnityEngine;

namespace GameFramework.View.UI
{
    public class FingerSelector : NavigationItemSelector
    {
        [SerializeField]
        private GameObject m_OutFocus;
        [SerializeField]
        private GameObject m_Finger;

        private void Awake()
        {
            if (m_Finger != null)
            {
                m_Finger.SetActive(false);
            }

            if (m_OutFocus != null)
            {
                m_OutFocus.SetActive(false);
            }
        }

        public override void OnSelect()
        {
            if(m_Finger != null) 
            {
                m_Finger.SetActive(true);            
            }

            if(m_OutFocus != null) 
            {            
                m_OutFocus.SetActive(false);
            }
        }

        public override void OnDeselect()
        {
            if (m_Finger != null)
            {
                m_Finger.SetActive(false);
            }
            if (m_OutFocus != null)
            {
                m_OutFocus.SetActive(false);
            }
        }

        public override void OnOutFocus()
        {
            if (m_Finger != null)
            {
                m_Finger.SetActive(false);
            }
            if (m_OutFocus != null)
            {
                m_OutFocus.SetActive(true);
            }
        }
    }
}
