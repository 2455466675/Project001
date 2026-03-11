using UnityEngine;

namespace GameFrameworkAOT.Core
{
    public class FsmDownloadPackageOver : IStateNode
    {
        private StateMachine m_Machine;

        void IStateNode.OnCreate(StateMachine machine)
        {
            m_Machine = machine;
        }
        void IStateNode.OnEnter()
        {
            Debug.Log("资源文件下载完毕！");
            m_Machine.ChangeState<FsmClearCacheBundle>();
        }
        void IStateNode.OnUpdate()
        {
        }
        void IStateNode.OnExit()
        {
        }
    }
}