using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GSystem
{
    public class Actor : MonoBehaviour
    {
        [ReadOnly]
        public int id;
        public SpriteRenderer spriteRenderer;       
        public Animator animator;
        public AnimatorController animatorController;
        [SerializeField]
        private Rigidbody m_rigidbody;
        public Rigidbody Rigidbody => m_rigidbody;
        public BoxCollider boxCollider;
        public ActorBones bones;
        public Transform hudNode;
        [SerializeField]
        private ActionDriver actionDriver;

        public void Reuse(int id) 
        {
            this.id = id;
            if (animatorController != null)
            {
                SwitchAnimatorController(animatorController.DefaultController);
            }            
        }

        public void Unuse() 
        {
        }

        public void SwitchAnimatorController(AnimatorControllerType controllerType) 
        {        
            if (animator == null || animatorController == null) 
            {
                return;
            }
            RuntimeAnimatorController controller = animatorController.GetController(controllerType);
            if (controller == null) 
            {
                return;
            }
            animator.runtimeAnimatorController = controller;
        }

        public Transform GetBone(string boneName) 
        {
            if (bones == null) 
            {
                return transform;
            }

            return bones.GetBone(boneName);
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

            if (m_rigidbody == null) 
            {
                m_rigidbody = GetComponentInChildren<Rigidbody>();
            }

            if (boxCollider == null) 
            {
                boxCollider = GetComponentInChildren<BoxCollider>();
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