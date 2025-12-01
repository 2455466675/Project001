using UnityEngine;

namespace GameFramework.Core
{
    public class Actor : MonoBehaviour
    {
        public int Id { get; set; }
        public ActorAnimator Animator => m_Animator;

        [SerializeField]
        private Rigidbody m_Rigidbody;
        [SerializeField]
        private Collider m_Collider;
        [SerializeField]
        private ActorAnimator m_Animator;
        [SerializeField]
        private Transform[] m_Bones;

        public Vector3 Velocity
        {
            get
            {
                if (m_Rigidbody == null)
                {
                    return Vector3.zero;
                }
                else
                {
                    return m_Rigidbody.linearVelocity;
                }
            }
            set
            {
                if (m_Rigidbody == null)
                {
                    return;
                }
                else
                {
                    m_Rigidbody.linearVelocity = value;
                }
            }
        }

        public Vector3 Position
        {
            get
            {
                return transform.position;
            }
            set
            {
                transform.position = value;
            }
        }

        public Vector3 LocalPosition
        {
            get
            {
                return transform.localPosition;
            }
            set
            {
                transform.localPosition = value;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                return transform.rotation;
            }
            set
            {
                transform.rotation = value;
            }
        }

        public void MovePosition(Vector3 pos)
        {
            if (m_Rigidbody == null)
            {
                return;
            }
            else
            {
                m_Rigidbody.MovePosition(pos);                
            }
        }

        public Transform GetBone(string name) 
        {
            if (m_Bones == null || m_Bones.Length == 0)
            {
                return null;
            }

            foreach (var item in m_Bones)
            {
                if (item.gameObject.name == name) 
                {
                    return item;
                }
            }

            return m_Bones[0];
        }

        public void SetKinematic(bool isKinematic)
        {
            if (m_Rigidbody != null)
            {
                m_Rigidbody.isKinematic = isKinematic;
            }

            if (m_Collider != null)
            {
                m_Collider.isTrigger = isKinematic;
            }
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (m_Animator == null)
            {
                m_Animator = GetComponentInChildren<ActorAnimator>();
            }
            if (m_Rigidbody == null)
            {
                m_Rigidbody = GetComponentInChildren<Rigidbody>();
            }
        }
#endif

    }
}