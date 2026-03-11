using UnityEngine;

namespace GameFrameworkAOT.Core 
{
    internal class FsmFinish : IStateNode
    {
        private YooAssetInitiator m_Owner;

        void IStateNode.OnCreate(StateMachine machine)
        {
            m_Owner = machine.Owner as YooAssetInitiator;
        }
        void IStateNode.OnEnter()
        {
            Debug.Log("资源热更结束！");
            m_Owner.SetFinish();
        }

        void IStateNode.OnUpdate()
        {
        }

        void IStateNode.OnExit()
        {
        }
    }
}
