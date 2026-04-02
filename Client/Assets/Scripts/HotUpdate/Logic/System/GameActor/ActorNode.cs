using UnityEngine;
using GameFramework.Core;

namespace GameFramework.Logic
{
    public enum ActorType
    {
        Pool,
        Party,
        Npc,
        Other,
    }

    public class ActorNode : GameNode
    {
        [SerializeField]
        private Transform m_PoolNode;
        [SerializeField]
        private Transform m_PartyNode;
        [SerializeField]
        private Transform m_NpcNode;
        [SerializeField]
        private Transform m_OtherNode;

        private void Awake()
        {
            if (m_PoolNode != null)
            {
                m_PoolNode.gameObject.SetActive(false);
            }
        }

        public Transform GetActorNode(ActorType nodeType)
        {
            switch (nodeType)
            {
                case ActorType.Pool:
                    return m_PoolNode;
                case ActorType.Party:
                    return m_PartyNode;
                case ActorType.Npc:
                    return m_NpcNode;
                case ActorType.Other:
                    return m_OtherNode;
                default:
                    return null;
            }
        }
    }
}
