using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.System
{
    public class Actor : MonoBehaviour
    {
        [ReadOnly]
        public int id;

        public SpriteRenderer spriteRenderer;
        
        public Animator animator;

        [SerializeField]
        private Rigidbody2D m_rigidbody2D;
        public Rigidbody2D Rigidbody2D => m_rigidbody2D;

        public BoxCollider2D boxCollider2D;

        public ActorBones bones;

        [SerializeField]
        private ActionDriver actionDriver;

        public void Reuse(int id) 
        {
            this.id = id;
        }

        public void Unuse() 
        {
            
        }

        public ActionHandle PlayAction(int hash, object userData) 
        {
            if (actionDriver == null) 
            {
                return null;
            }
            ActionHandle handle = actionDriver.PlayAction(hash, this, userData);
            return handle;
        }

        public void SetParent(Transform parent) 
        {
            transform.SetParent(parent);
        }

        [Button("Init")]
        private void Init() 
        {
            if (spriteRenderer == null) 
            {
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }

            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }

            if (m_rigidbody2D == null) 
            {
                m_rigidbody2D = GetComponentInChildren<Rigidbody2D>();
            }

            if (boxCollider2D == null) 
            {
                boxCollider2D = GetComponentInChildren<BoxCollider2D>();
            }

            if (bones == null)
            {
                bones = GetComponentInChildren<ActorBones>();
            }

            if (actionDriver == null)
            {
                actionDriver = GetComponentInChildren<ActionDriver>();
            }
        }
    }
}