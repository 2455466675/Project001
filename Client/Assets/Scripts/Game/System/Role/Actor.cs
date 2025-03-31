using UnityEngine;

namespace Game.System
{
    public class Actor : MonoBehaviour
    {        
        public SpriteRenderer spriteRenderer;
        
        public Animator animator;

        public new Rigidbody2D rigidbody2D;

        public BoxCollider2D boxCollider2D;
    }
}