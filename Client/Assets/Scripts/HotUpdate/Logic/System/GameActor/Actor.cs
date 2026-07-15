using UnityEngine;

namespace GameFramework.Logic
{
    public class Actor : MonoBehaviour, IActor
    {
        public int Id { get; set; }

        [SerializeField]
        private Rigidbody m_Rigidbody;

        [SerializeField]
        private Puppet puppet;

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetVelocity(Vector3 velocity)
        {
            if (m_Rigidbody != null)
            {
                m_Rigidbody.linearVelocity = velocity;
            }
        }

        public void MovePosition(Vector3 pos)
        {
            if (m_Rigidbody != null)
            {
                m_Rigidbody.MovePosition(pos);
            }            
        }

        #region Puppet

        public void SetAnimatorController(string name)
        {
            if (puppet == null)
            {
                return;
            }
            puppet.SetAnimatorController(name);
        }

        public void SetAnimatorValue(string name, bool value)
        {
            if (puppet == null)
            {
                return;
            }
            puppet.SetAnimatorValue(name, value);
        }

        public void SetAnimatorValue(string name, float value)
        {
            if (puppet == null)
            {
                return;
            }
            puppet.SetAnimatorValue(name, value);
        }

        public void SetAnimatorValue(string name, int value)
        {
            if (puppet == null)
            {
                return;
            }
            puppet.SetAnimatorValue(name, value);
        }

        public void SetAnimatorValue(string name)
        {
            if (puppet == null)
            {
                return;
            }
            puppet.SetAnimatorValue(name);
        }

        public float PlayAnimation(string name)
        {
            if (puppet == null)
            {
                return 0f;
            }
            return puppet.PlayAnimation(name);
        }

        public void AddWidget(PuppetWidgetArgs args)
        {
            if (puppet == null)
            {
                return;
            }
            puppet.AddWidget(args);
        }

        public void RemoveWidget(string key)
        {
            if (puppet == null)
            {
                return;
            }
            puppet.RemoveWidget(key);
        }

        #endregion

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (m_Rigidbody == null)
            {
                m_Rigidbody = GetComponentInChildren<Rigidbody>();
            }
        }
#endif
    }
}
