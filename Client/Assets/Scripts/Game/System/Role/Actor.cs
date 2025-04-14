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

        public ActionHandle PlayAction(string actionName, object userData = null) 
        {
            if (actionDriver == null) 
            {
                return null;
            }
            return actionDriver.PlayAction(actionName, this, userData);
        }
    }
}