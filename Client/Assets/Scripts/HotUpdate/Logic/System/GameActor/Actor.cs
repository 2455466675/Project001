using UnityEngine;

namespace GameFramework.Logic
{
    public class Actor : MonoBehaviour, IActor
    {
        public int Id { get; set; }

        [SerializeField]
        private Rigidbody m_Rigidbody;
        [SerializeField]
        private Collider m_Collider;
        [SerializeField]
        private ActorAnimator m_Animator;
        [SerializeField]
        private Transform[] m_Bones;

        public Transform Transform => transform;

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

        public void SetAnimatorController(string name)
        {
            if (m_Animator == null)
            {
                return;
            }
            m_Animator.SetAnimatorController(name);
        }

        public void SetBool(string name, bool value)
        {
            if (m_Animator == null)
            {
                return;
            }
            m_Animator.SetBool(name, value);
        }

        public void SetFloat(string name, float value)
        {
            if (m_Animator == null)
            {
                return;
            }
            m_Animator.SetFloat(name, value);
        }

        public void SetInteger(string name, int value)
        {
            if (m_Animator == null)
            {
                return;
            }
            m_Animator.SetInteger(name, value);
        }

        public void SetTrigger(string name)
        {
            if (m_Animator == null)
            {
                return;
            }
            m_Animator.SetTrigger(name);
        }

        public void PlayAnim(string name)
        {
            if (m_Animator == null)
            {
                return;
            }
            m_Animator.PlayAnim(name);
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
            if (m_Collider == null)
            {
                m_Collider = GetComponentInChildren<Collider>();
            }
        }
#endif
    }
}