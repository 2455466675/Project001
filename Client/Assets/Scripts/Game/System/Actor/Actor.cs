using UnityEngine;

namespace Game.System
{
    public class Actor : MonoBehaviour
    {        
        public SpriteRenderer spriteRenderer;
        
        public Animator animator;

        [SerializeField]
        private Rigidbody2D m_rigidbody2D;
        public Rigidbody2D Rigidbody2D => m_rigidbody2D;

        public BoxCollider2D boxCollider2D;


        [SerializeField]
        private ActionDriver actionDriver;

        public ActionHandle PlayAction(int hash, object userData) 
        {
            if (actionDriver == null) 
            {
                return null;
            }
            ActionHandle handle = actionDriver.PlayAction(hash, this, userData);
            return handle;
        }
    }
}