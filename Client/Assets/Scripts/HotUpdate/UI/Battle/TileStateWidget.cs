using GameFramework.Gameplay;
using UnityEngine;

namespace GameFramework.UI
{
    public class TileStateWidget : UIWidget
    {
        [SerializeField]
        private SpriteRenderer m_Green;
        [SerializeField]
        private SpriteRenderer m_Red;
        [SerializeField]
        private SpriteRenderer m_Yellow;
        [SerializeField]
        private SpriteRenderer m_Blue;
        [SerializeField]
        private SpriteRenderer m_Border;

        public void SetState(int state)
        {
            m_Green.gameObject.SetActive(state == TileState.Green);
            m_Red.gameObject.SetActive(state == TileState.Red);
            m_Yellow.gameObject.SetActive(state == TileState.Yellow);
            m_Blue.gameObject.SetActive(state == TileState.Blue);
        }
    }
}
